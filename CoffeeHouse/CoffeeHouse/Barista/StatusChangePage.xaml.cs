using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeHouse.Barista
{
    public partial class StatusChangePage : Page
    {
        public ObservableCollection<OrderStatus> Orders { get; set; }

        public StatusChangePage()
        {
            InitializeComponent();
            Orders = new ObservableCollection<OrderStatus>();
            OrdersGrid.ItemsSource = Orders;

            LoadOrdersFromDatabase();
        }

        private void LoadOrdersFromDatabase()
        {
            try
            {
                using (var db = new CoffeeHouseEntities())
                {
                    var dbOrders = db.Заказы
                        .OrderByDescending(o => o.ВремяСоздания)
                        .ToList();

                    Orders.Clear();

                    foreach (var dbOrder in dbOrders)
                    {
                        var availableStatuses = new ObservableCollection<string>
                        {
                            "в ожидании",
                            "в процессе",
                            "завершен",
                            "отменен"
                        };

                        // Если статус из БД не входит в стандартные, добавляем его
                        if (!string.IsNullOrEmpty(dbOrder.Статус) &&
                            !availableStatuses.Contains(dbOrder.Статус.ToLower()))
                        {
                            availableStatuses.Add(dbOrder.Статус);
                        }

                        Orders.Add(new OrderStatus
                        {
                            OrderId = dbOrder.IDЗаказа,
                            OrderNumber = dbOrder.Номер_заказа,
                            OrderType = dbOrder.ТипЗаказа,
                            CreationTime = dbOrder.ВремяСоздания.ToString("HH:mm dd.MM.yyyy"),
                            Status = dbOrder.Статус?.ToLower() ?? "в ожидании",
                            AvailableStatuses = availableStatuses
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заказов: {ex.Message}");
            }
        }

        private void UpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = (button.DataContext as OrderStatus) ??
                       (OrdersGrid.SelectedItem as OrderStatus);

            if (order == null)
            {
                MessageBox.Show("Выберите заказ для обновления статуса");
                return;
            }

            try
            {
                using (var db = new CoffeeHouseEntities())
                {
                    var dbOrder = db.Заказы.Find(order.OrderId);
                    if (dbOrder != null)
                    {
                        // Сохраняем статус в том виде, как он есть (без приведения к нижнему регистру)
                        dbOrder.Статус = order.Status;
                        db.Entry(dbOrder).State = EntityState.Modified;
                        db.SaveChanges();

                        // Локально обновляем статус без полной перезагрузки
                        var updatedOrder = Orders.FirstOrDefault(o => o.OrderId == order.OrderId);
                        if (updatedOrder != null)
                        {
                            updatedOrder.Status = order.Status;
                        }

                        MessageBox.Show($"Статус заказа {order.OrderNumber} изменен на {order.Status}",
                                      "Успешно",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении статуса: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadOrdersFromDatabase();
        }
    }

    public class OrderStatus
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string OrderType { get; set; }
        public string CreationTime { get; set; }
        public string Status { get; set; }
        public ObservableCollection<string> AvailableStatuses { get; set; }
    }
}