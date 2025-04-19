using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeHouse.Barista
{
    public partial class PreparationLogPage : Page
    {
        public ObservableCollection<PreparationLog> Logs { get; set; }

        public PreparationLogPage()
        {
            InitializeComponent();
            Logs = new ObservableCollection<PreparationLog>();
            PreparationLogGrid.ItemsSource = Logs;

            // Устанавливаем даты по умолчанию (сегодня)
            StartDatePicker.SelectedDate = DateTime.Today;
            EndDatePicker.SelectedDate = DateTime.Today;

            LoadLogsFromDatabase();
        }

        private void LoadLogsFromDatabase()
        {
            try
            {
                using (var db = new CoffeeHouseEntities())
                {
                    var startDate = StartDatePicker.SelectedDate ?? DateTime.Today;
                    var endDate = EndDatePicker.SelectedDate ?? DateTime.Today;

                    // Получаем заказы за выбранный период
                    var orders = db.Заказы
                        .Where(o => o.ВремяСоздания != null &&
                                   DbFunctions.TruncateTime(o.ВремяСоздания) >= startDate.Date &&
                                   DbFunctions.TruncateTime(o.ВремяСоздания) <= endDate.Date)
                        .OrderByDescending(o => o.ВремяСоздания)
                        .ToList();

                    Logs.Clear();

                    foreach (var order in orders)
                    {
                        // Получаем позиции заказа
                        var orderItems = db.ПозицииЗаказа
                            .Where(p => p.IDЗаказа == order.IDЗаказа)
                            .Join(db.Меню,
                                position => position.IDТовара,
                                menu => menu.IDТовара,
                                (position, menu) => new { position, menu })
                            .ToList();

                        foreach (var item in orderItems)
                        {
                            Logs.Add(new PreparationLog
                            {
                                Date = order.ВремяСоздания.ToString("yyyy-MM-dd"),
                                OrderNumber = order.Номер_заказа,
                                DrinkName = item.menu.Название,
                                PreparationTime = item.menu.ВремяПриготовления ?? "Не указано",
                                Status = "Завершен"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке журнала: {ex.Message}");
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartDatePicker.SelectedDate == null || EndDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите даты для фильтрации");
                return;
            }

            if (StartDatePicker.SelectedDate > EndDatePicker.SelectedDate)
            {
                MessageBox.Show("Дата начала не может быть больше даты окончания");
                return;
            }

            LoadLogsFromDatabase();
        }
    }

    public class PreparationLog
    {
        public string Date { get; set; }
        public string OrderNumber { get; set; }
        public string DrinkName { get; set; }
        public string PreparationTime { get; set; }
        public string Status { get; set; }
    }
}