using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using OfficeOpenXml;
using Page = System.Windows.Controls.Page;

namespace CoffeeHouse.Manager
{
    public partial class ManagerReportsPage : Page
    {
        public ManagerReportsPage()
        {
            InitializeComponent();

            // Устанавливаем даты по умолчанию
            SalesStartDate.SelectedDate = DateTime.Today.AddDays(-7);
            SalesEndDate.SelectedDate = DateTime.Today;
            OrdersStartDate.SelectedDate = DateTime.Today.AddMonths(-1);
            OrdersEndDate.SelectedDate = DateTime.Today;

            SalesGroupingCombo.SelectedIndex = 0;
            InventoryCategoryCombo.SelectedIndex = 0;
            SupplierCombo.SelectedIndex = 0;
        }

        private void GenerateSalesReport_Click(object sender, RoutedEventArgs e)
        {
            if (SalesStartDate.SelectedDate == null || SalesEndDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты для формирования отчета");
                return;
            }

            var startDate = SalesStartDate.SelectedDate.Value;
            var endDate = SalesEndDate.SelectedDate.Value;

            using (var db = new CoffeeHouseEntities())
            {
                var orders = db.Заказы
                    .Where(o => o.ВремяСоздания >= startDate && o.ВремяСоздания <= endDate && o.ИтоговаяСумма != null)
                    .ToList();

                if (orders.Count == 0)
                {
                    MessageBox.Show("Нет данных для отображения за выбранный период");
                    return;
                }

                var reportData = new List<SalesReportItem>();

                switch (SalesGroupingCombo.SelectedIndex)
                {
                    case 0: // По дням
                        reportData = orders
                            .GroupBy(o => o.ВремяСоздания.Date)
                            .Select(g => new SalesReportItem
                            {
                                Период = g.Key.ToShortDateString(),
                                КоличествоЗаказов = g.Count(),
                                Выручка = g.Sum(o => o.ИтоговаяСумма ?? 0),
                                СреднийЧек = g.Average(o => o.ИтоговаяСумма ?? 0)
                            })
                            .OrderBy(r => DateTime.Parse(r.Период))
                            .ToList();
                        break;
                    case 1: // По неделям
                        reportData = orders
                            .GroupBy(o => new { Year = o.ВремяСоздания.Year, Week = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(o.ВремяСоздания.Date, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday) })
                            .Select(g => new SalesReportItem
                            {
                                Период = $"Неделя {g.Key.Week}, {g.Key.Year}",
                                КоличествоЗаказов = g.Count(),
                                Выручка = g.Sum(o => o.ИтоговаяСумма ?? 0),
                                СреднийЧек = g.Average(o => o.ИтоговаяСумма ?? 0)
                            })
                            .OrderBy(r => r.Период)
                            .ToList();
                        break;
                    case 2: // По месяцам
                        reportData = orders
                            .GroupBy(o => new { o.ВремяСоздания.Year, o.ВремяСоздания.Month })
                            .Select(g => new SalesReportItem
                            {
                                Период = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMMM yyyy"),
                                КоличествоЗаказов = g.Count(),
                                Выручка = g.Sum(o => o.ИтоговаяСумма ?? 0),
                                СреднийЧек = g.Average(o => o.ИтоговаяСумма ?? 0)
                            })
                            .OrderBy(r => DateTime.Parse(r.Период))
                            .ToList();
                        break;
                }

                SalesReportGrid.ItemsSource = reportData;
            }
        }

        private void GenerateInventoryReport_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new CoffeeHouseEntities())
            {
                // Получаем все товары из меню (а не ингредиенты)
                var menuItems = db.Меню.ToList();

                // Если выбрана конкретная категория (не "Все категории")
                if (InventoryCategoryCombo.SelectedIndex > 0)
                {
                    string selectedCategory = (InventoryCategoryCombo.SelectedItem as ComboBoxItem)?.Content.ToString();

                    if (!string.IsNullOrEmpty(selectedCategory))
                    {
                        // Фильтруем по категории товара
                        menuItems = menuItems
                            .Where(m => m.Категория != null &&
                                       m.Категория.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                    }
                }

                // Получаем остатки ингредиентов для этих товаров
                var reportData = new List<InventoryReportItem>();

                foreach (var menuItem in menuItems)
                {
                    // Получаем состав блюда (ингредиенты)
                    var composition = db.СоставБлюда
                        .Where(s => s.IDТовара == menuItem.IDТовара)
                        .ToList();

                    foreach (var item in composition)
                    {
                        var ingredient = db.Ингредиенты.FirstOrDefault(i => i.IDИнгредиента == item.IDИнгредиента);

                        if (ingredient != null)
                        {
                            reportData.Add(new InventoryReportItem
                            {
                                Товар = menuItem.Название,
                                Категория = menuItem.Категория,
                                Остаток = ingredient.ТекущийОстаток,
                                ЕдиницаИзмерения = ingredient.ЕдиницаИзмерения,
                                СреднийРасход = CalculateAverageConsumption(ingredient.IDИнгредиента, db),
                                ДнейХватит = CalculateDaysLeft(ingredient.IDИнгредиента, db)
                            });
                        }
                    }
                }

                InventoryReportGrid.ItemsSource = reportData;
            }
        }

        private string CalculateAverageConsumption(int ingredientId, CoffeeHouseEntities db)
        {
            
            // среднее количество использования ингредиента за последние 30 дней
            return "10"; 
        }

        private string CalculateDaysLeft(int ingredientId, CoffeeHouseEntities db)
        {
            
            //  текущий остаток / средний дневной расход
            return "15"; 
        }

        private void GenerateSupplierOrdersReport_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersStartDate.SelectedDate == null || OrdersEndDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты для формирования отчета");
                return;
            }

            var startDate = OrdersStartDate.SelectedDate.Value;
            var endDate = OrdersEndDate.SelectedDate.Value;

            using (var db = new CoffeeHouseEntities())
            {
                // Получаем все заказы поставщикам за указанный период
                var supplierOrders = db.Заказы
      .AsNoTracking()
      .Where(o => o.ТипЗаказа == "Заказ поставщику" &&
                 o.ВремяСоздания >= startDate &&
                 o.ВремяСоздания <= endDate)
      .ToList();

                // Если выбран конкретный поставщик
                if (SupplierCombo.SelectedIndex > 0)
                {
                    // Получаем полное название поставщика из ComboBox
                    string selectedSupplier = (SupplierCombo.SelectedItem as ComboBoxItem)?.Content.ToString();

                    if (!string.IsNullOrEmpty(selectedSupplier))
                    {
                        supplierOrders = supplierOrders
                            .Where(o => o.КомментарийКлиента != null &&
                                       o.КомментарийКлиента.Contains(selectedSupplier))
                            .ToList();
                    }
                }

                // Формируем отчет
                var reportData = supplierOrders.Select(o => new SupplierOrderReportItem
                {
                    НомерЗаявки = o.Номер_заказа ?? o.IDЗаказа.ToString(),
                    Дата = o.ВремяСоздания.ToShortDateString(),
                    Поставщик = o.КомментарийКлиента ?? "Не указан",
                    Сумма = o.ИтоговаяСумма ?? 0,
                    Статус = o.Статус ?? "В обработке"
                }).ToList();

                SupplierOrdersReportGrid.ItemsSource = reportData;

                if (reportData.Count == 0)
                {
                    MessageBox.Show("Нет данных за выбранный период");
                }
            }
        }
        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var data = SalesReportGrid.ItemsSource as IEnumerable<dynamic>;
                if (data == null || !data.Any())
                {
                    MessageBox.Show("Нет данных для экспорта", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = true;
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Worksheet)workbook.Worksheets[1];

                

                // Заголовки
                worksheet.Cells[1, 1].Value = "Период";
                worksheet.Cells[1, 2].Value = "Кол-во заказов";
                worksheet.Cells[1, 3].Value = "Выручка";
                worksheet.Cells[1, 4].Value = "Средний чек";

               
                // Данные
                var items = SalesReportGrid.ItemsSource as IEnumerable<SalesReportItem>;
                int row = 2;
                foreach (var item in items)
                {
                    worksheet.Cells[row, 1].Value = item.Период;
                    worksheet.Cells[row, 2].Value = item.КоличествоЗаказов;
                    worksheet.Cells[row, 3].Value = item.Выручка;
                    worksheet.Cells[row, 4].Value = item.СреднийЧек;
                    row++;
                }

                // Форматирование
                worksheet.Columns.AutoFit();
                ((Range)worksheet.Rows[1]).Font.Bold = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в Excel: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
      
        
    }

    // Классы для хранения данных отчетов
    public class SalesReportItem
    {
        public string Период { get; set; }
        public int КоличествоЗаказов { get; set; }
        public decimal Выручка { get; set; }
        public decimal СреднийЧек { get; set; }
    }

    public class InventoryReportItem
    {
        public string Товар { get; set; }
        public string Категория { get; set; }
        public string Остаток { get; set; }
        public string ЕдиницаИзмерения { get; set; }
        public string СреднийРасход { get; set; }
        public string ДнейХватит { get; set; }
    }

    public class SupplierOrderReportItem
    {
        public string НомерЗаявки { get; set; }
        public string Дата { get; set; }
        public string Поставщик { get; set; }
        public decimal Сумма { get; set; }
        public string Статус { get; set; }
    }
}