using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using Application = Microsoft.Office.Interop.Excel.Application;
using Page = System.Windows.Controls.Page;
using Range = Microsoft.Office.Interop.Excel.Range;

namespace CoffeeHouse.Admin
{
    public partial class ReportsPage : Page
    {
        private CoffeeHouseEntities _db;

        public ReportsPage()
        {
            InitializeComponent();
            _db = new CoffeeHouseEntities();

            // Установка дат по умолчанию
            StartDatePicker.SelectedDate = DateTime.Today.AddDays(-7);
            EndDatePicker.SelectedDate = DateTime.Today;
        }

        private void GenerateSalesReport_Click(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты для формирования отчета", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime startDate = StartDatePicker.SelectedDate.Value;
            DateTime endDate = EndDatePicker.SelectedDate.Value;

            try
            {
                var reportType = (ReportTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                var salesData = _db.Заказы
                    .Where(z => z.ВремяСоздания >= startDate && z.ВремяСоздания <= endDate)
                    .AsEnumerable()
                    .GroupBy(z =>
                    {
                        DateTime date = z.ВремяСоздания; // No null check needed since it's not nullable

                        if (reportType == "По дням") return date.Date;
                        if (reportType == "По неделям")
                            return date.Date.AddDays(-(int)date.DayOfWeek);
                        if (reportType == "По месяцам")
                            return new DateTime(date.Year, date.Month, 1);

                        return date.Date;
                    })
                    .Select(g => new
                    {
                        Дата = g.Key,
                        Заказы = g.Count(),
                        Выручка = g.Sum(z => z.ИтоговаяСумма ?? 0)
                    })
                    .OrderBy(x => x.Дата)
                    .ToList();

                SalesReportGrid.ItemsSource = salesData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании отчета: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GenerateTopItemsReport_Click(object sender, RoutedEventArgs e)
        {
           
                var reportType = (TopItemsComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                IEnumerable<dynamic> result;

                switch (reportType)
                {
                    case "ТОП-10 по количеству продаж":
                        result = _db.ПозицииЗаказа
                            .AsEnumerable()
                            .GroupBy(p => p.IDТовара)
                            .Select(g => new
                            {
                                IDТовара = g.Key,
                                Продажи = g.Sum(p => int.Parse(p.Количество)),
                                Выручка = g.Sum(p => p.ЦенаНаМомент * int.Parse(p.Количество))
                            })
                            .OrderByDescending(x => x.Продажи)
                            .Take(10)
                            .Join(_db.Меню,
                                p => p.IDТовара,
                                m => m.IDТовара,
                                (p, m) => new
                                {
                                    Позиция = 0,
                                    m.Название,
                                    m.Категория,
                                    p.Продажи,
                                    p.Выручка
                                })
                            .ToList();
                        break;

                    case "ТОП-10 по выручке":
                        result = _db.ПозицииЗаказа
                            .AsEnumerable()
                            .GroupBy(p => p.IDТовара)
                            .Select(g => new
                            {
                                IDТовара = g.Key,
                                Продажи = g.Sum(p => int.Parse(p.Количество)),
                                Выручка = g.Sum(p => p.ЦенаНаМомент * int.Parse(p.Количество))
                            })
                            .OrderByDescending(x => x.Выручка)
                            .Take(10)
                            .Join(_db.Меню,
                                p => p.IDТовара,
                                m => m.IDТовара,
                                (p, m) => new
                                {
                                    Позиция = 0,
                                    m.Название,
                                    m.Категория,
                                    p.Продажи,
                                    p.Выручка
                                })
                            .ToList();
                        break;

                    case "Наименее популярные":
                        result = _db.ПозицииЗаказа
                            .AsEnumerable()
                            .GroupBy(p => p.IDТовара)
                            .Select(g => new
                            {
                                IDТовара = g.Key,
                                Продажи = g.Sum(p => int.Parse(p.Количество)),
                                Выручка = g.Sum(p => p.ЦенаНаМомент * int.Parse(p.Количество))
                            })
                            .OrderBy(x => x.Продажи)
                            .Take(10)
                            .Join(_db.Меню,
                                p => p.IDТовара,
                                m => m.IDТовара,
                                (p, m) => new
                                {
                                    Позиция = 0,
                                    m.Название,
                                    m.Категория,
                                    p.Продажи,
                                    p.Выручка
                                })
                            .ToList();
                        break;

                    default: // ТОП-10 по маржинальности 
                    result = _db.СоставБлюда
                        .AsEnumerable()
                        .GroupBy(s => s.IDТовара)
                        .Select(g => new
                        {
                            IDТовара = g.Key,
                            Себестоимость = g.Sum(s => {
                                decimal quantity;
                                decimal cost;
                                bool quantityParsed = decimal.TryParse(s.Количество, out quantity);
                                bool costParsed = decimal.TryParse(
                                    _db.Ингредиенты.First(i => i.IDИнгредиента == s.IDИнгредиента).СебестоимостьЗаЕдиницу,
                                    out cost);

                                return quantityParsed && costParsed ? quantity * cost : 0m;
                            })
                        })
                        .Join(_db.Меню,
                            s => s.IDТовара,
                            m => m.IDТовара,
                            (s, m) => new
                            {
                                m.IDТовара,
                                m.Название,
                                m.Категория,
                                s.Себестоимость,
                                m.Цена
                            })
                        .Join(_db.ПозицииЗаказа
                                .AsEnumerable()
                                .GroupBy(p => p.IDТовара)
                                .Select(g => new
                                {
                                    IDТовара = g.Key,
                                    Продажи = g.Sum(p => {
                                        int quantity;
                                        return int.TryParse(p.Количество, out quantity) ? quantity : 0;
                                    }),
                                    Выручка = g.Sum(p => {
                                        int quantity;
                                        return int.TryParse(p.Количество, out quantity)
                                            ? p.ЦенаНаМомент * quantity
                                            : 0m;
                                    })
                                }),
                            m => m.IDТовара,
                            p => p.IDТовара,
                            (m, p) => new
                            {
                                Позиция = 0,
                                m.Название,
                                m.Категория,
                                p.Продажи,
                                p.Выручка,
                                Маржинальность = (m.Цена - m.Себестоимость) * p.Продажи
                            })
                        .OrderByDescending(x => x.Маржинальность)
                        .Take(10)
                        .Select(x => new
                        {
                            x.Позиция,
                            x.Название,
                            x.Категория,
                            x.Продажи,
                            x.Выручка
                        })
                        .ToList();
                    break;
            }

                var positionedResult = result.Select((item, index) => new
                {
                    Позиция = index + 1,
                    item.Название,
                    item.Категория,
                    item.Продажи,
                    item.Выручка
                }).ToList();

                TopItemsGrid.ItemsSource = positionedResult;
            
            
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

                var excelApp = new Application();
                excelApp.Visible = true;
                var workbook = excelApp.Workbooks.Add();
                var worksheet = (Worksheet)workbook.Worksheets[1];

                // Заголовки
                worksheet.Cells[1, 1] = "Дата";
                worksheet.Cells[1, 2] = "Количество заказов";
                worksheet.Cells[1, 3] = "Выручка";

                // Данные
                int row = 2;
                foreach (var item in data)
                {
                    worksheet.Cells[row, 1] = item.Дата?.ToString("d") ?? "";
                    worksheet.Cells[row, 2] = item.Заказы;
                    worksheet.Cells[row, 3] = item.Выручка;
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
}