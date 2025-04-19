using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CoffeeHouse.Barista
{
    public partial class OrdersQueuePage : Page
    {
        public ObservableCollection<Order> Orders { get; set; }

        public OrdersQueuePage()
        {
            InitializeComponent();
            Orders = new ObservableCollection<Order>();
            OrdersQueueList.ItemsSource = Orders;

            LoadOrdersFromDatabase();
        }

        private void LoadOrdersFromDatabase()
        {
            using (var db = new CoffeeHouseEntities())
            {
                // Получаем все заказы, которые еще не завершены
                var dbOrders = db.Заказы
                    .Where(o => o.ВремяЗавершения == null)
                    .OrderBy(o => o.ВремяСоздания)
                    .ToList();

                foreach (var dbOrder in dbOrders)
                {
                    // Получаем позиции для каждого заказа
                    var orderItems = db.ПозицииЗаказа
     .Where(p => p.IDЗаказа == dbOrder.IDЗаказа)
     .Join(db.Меню,
         position => position.IDТовара,
         menu => menu.IDТовара,
         (position, menu) => new { position, menu })
     .AsEnumerable() // Переключаемся на LINQ to Objects
     .Select(x => $"{x.menu.Название} x{x.position.Количество}" +
         (!string.IsNullOrEmpty(x.position.Модификация) ? $" ({x.position.Модификация})" : ""))
     .ToList();

                    // Определяем статус заказа
                    string status;
                    Brush statusColor;
                    Visibility canTakeToWork;

                    if (dbOrder.Статус == "в процессе")
                    {
                        status = "в процессе";
                        statusColor = Brushes.Orange;
                        canTakeToWork = Visibility.Collapsed;
                    }
                    else if (dbOrder.Статус == "завершен")
                    {
                        status = "завершен";
                        statusColor = Brushes.Green;
                        canTakeToWork = Visibility.Collapsed;
                    }
                    else
                    {
                        status = "в ожидании";
                        statusColor = Brushes.OrangeRed;
                        canTakeToWork = Visibility.Visible;
                    }

                    Orders.Add(new Order
                    {
                        OrderId = dbOrder.IDЗаказа,
                        OrderNumber = dbOrder.Номер_заказа,
                        OrderTime = dbOrder.ВремяСоздания.ToString("yyyy:HH:mm"),
                        Status = status,
                        StatusColor = statusColor,
                        Items = new ObservableCollection<string>(orderItems),
                        CanTakeToWork = canTakeToWork,
                        OrderType = dbOrder.ТипЗаказа,
                        PaymentMethod = dbOrder.СпособОплаты,
                        Comment = dbOrder.КомментарийКлиента
                    });
                }
            }
        }

        private void TakeOrder_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button.DataContext as Order;

            using (var db = new CoffeeHouseEntities())
            {
                var dbOrder = db.Заказы.FirstOrDefault(o => o.IDЗаказа == order.OrderId);
                if (dbOrder != null)
                {
                    dbOrder.Статус = "в процессе";
                    db.SaveChanges();
                }
            }

            order.Status = "в процессе";
            order.StatusColor = Brushes.Orange;
            order.CanTakeToWork = Visibility.Collapsed;

            MessageBox.Show($"Заказ {order.OrderNumber} взят в работу");
        }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string OrderTime { get; set; }
        public string Status { get; set; }
        public Brush StatusColor { get; set; }
        public ObservableCollection<string> Items { get; set; } = new ObservableCollection<string>();
        public Visibility CanTakeToWork { get; set; }
        public string OrderType { get; set; }
        public string PaymentMethod { get; set; }
        public string Comment { get; set; }
    }
}