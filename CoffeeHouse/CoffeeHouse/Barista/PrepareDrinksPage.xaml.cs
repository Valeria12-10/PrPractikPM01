using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System;
using System.Linq;
using System.Windows.Threading;

namespace CoffeeHouse.Barista
{
    public partial class PrepareDrinksPage : Page
    {
        public ObservableCollection<DrinkOrder> CurrentOrders { get; set; }

        public PrepareDrinksPage()
        {
            InitializeComponent();
            CurrentOrders = new ObservableCollection<DrinkOrder>();
            DataContext = this; // Устанавливаем DataContext для привязки данных

            LoadSampleData();
            InitializeDrinksMenu();
        }

        // Правильное объявление словарей для таймеров
        private Dictionary<int, DispatcherTimer> orderTimers = new Dictionary<int, DispatcherTimer>();
        private Dictionary<int, DateTime> orderStartTimes = new Dictionary<int, DateTime>();

        private void LoadSampleData()
        {
            using (var db = new CoffeeHouseEntities())
            {
                var activeOrders = db.Заказы
                    .Where(o => o.ВремяЗавершения == null) // Только незавершенные заказы
                    .ToList();

                CurrentOrders.Clear();

                foreach (var order in activeOrders)
                {
                    var orderItems = db.ПозицииЗаказа
                        .Where(p => p.IDЗаказа == order.IDЗаказа)
                        .Join(db.Меню,
                            position => position.IDТовара,
                            menu => menu.IDТовара,
                            (position, menu) => new
                            {
                                menu.Название,
                                menu.Категория,
                                position.Модификация,
                                position.Количество
                            })
                        .ToList();

                    foreach (var item in orderItems)
                    {
                        CurrentOrders.Add(new DrinkOrder
                        {
                            OrderId = order.IDЗаказа, // Добавляем ID заказа для связи
                            DrinkName = item.Название,
                            OrderNumber = order.Номер_заказа,
                            SpecialRequests = string.IsNullOrEmpty(item.Модификация)
                                ? order.КомментарийКлиента ?? ""
                                : $"{order.КомментарийКлиента} ({item.Модификация})",
                            TimeElapsed = (order.ВремяСоздания != DateTime.MinValue)
                                ? CalculateTimeElapsed(order.ВремяСоздания)
                                : "00:00:00",
                            //   DrinkImage = GetDrinkImage(item.Категория),
                            Quantity = item.Количество,
                            IsInProgress = (order.ВремяСоздания != DateTime.MinValue) && (order.ВремяЗавершения == null)
                        });
                    }
                }
            }
        }

        private void StartPreparation_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button.DataContext as DrinkOrder;

            // Обновляем статус в UI
            order.IsInProgress = true;

            // Запускаем таймер для этого заказа
            if (!orderTimers.ContainsKey(order.OrderId))
            {
                var timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += (s, args) => UpdateOrderTime(order.OrderId);
                timer.Start();

                orderTimers[order.OrderId] = timer;
                orderStartTimes[order.OrderId] = DateTime.Now;

                // Обновляем время создания в БД, если оно не установлено
                using (var db = new CoffeeHouseEntities())
                {
                    var dbOrder = db.Заказы.FirstOrDefault(o => o.IDЗаказа == order.OrderId);
                    if (dbOrder != null && dbOrder.ВремяСоздания == DateTime.MinValue)
                    {
                        dbOrder.ВремяСоздания = DateTime.Now;
                        db.SaveChanges();
                    }
                }
            }

            MessageBox.Show($"Начато приготовление: {order.DrinkName}");
        }

        private void UpdateOrderTime(int orderId)
        {
            // Находим все элементы этого заказа в CurrentOrders
            var ordersToUpdate = CurrentOrders.Where(o => o.OrderId == orderId).ToList();

            if (ordersToUpdate.Any() && orderStartTimes.TryGetValue(orderId, out var startTime))
            {
                string elapsedTime = CalculateTimeElapsed(startTime);

                foreach (var order in ordersToUpdate)
                {
                    order.TimeElapsed = elapsedTime;
                }
            }
        }

        private void CompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var order = button.DataContext as DrinkOrder;

            // Останавливаем таймер
            if (orderTimers.TryGetValue(order.OrderId, out var timer))
            {
                timer.Stop();
                orderTimers.Remove(order.OrderId);
                orderStartTimes.Remove(order.OrderId);
            }

            // Сохраняем время завершения в БД
            using (var db = new CoffeeHouseEntities())
            {
                var dbOrder = db.Заказы.FirstOrDefault(o => o.IDЗаказа == order.OrderId);
                if (dbOrder != null)
                {
                    dbOrder.ВремяЗавершения = DateTime.Now;
                    db.SaveChanges();
                }
            }

            // Обновляем статус в UI
            order.IsInProgress = false;
            MessageBox.Show($"Заказ {order.OrderNumber} завершен!");
        }

        // Метод для расчета прошедшего времени
        private string CalculateTimeElapsed(DateTime startTime)
        {
            TimeSpan elapsed = DateTime.Now - startTime;
            return $"{elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}";
        }


        private void InitializeDrinksMenu()
        {
            try
            {
                DrinksMenuPanel.Children.Clear(); // Очищаем панель перед загрузкой

                using (var db = new CoffeeHouseEntities())
                {
                    // Проверка подключения
                    if (!db.Database.Exists())
                    {
                        MessageBox.Show("Нет подключения к базе данных");
                        return;
                    }

                    var menuItems = db.Меню
                        .Where(item => item.Доступность == "доступен")
                        .OrderBy(item => item.Категория)
                        .ThenBy(item => item.Название)
                        .ToList();

                    if (!menuItems.Any())
                    {
                        MessageBox.Show("Меню пустое или нет доступных товаров");
                        return;
                    }

                    foreach (var drink in menuItems)
                    {
                        var card = new Border
                        {
                            Style = (Style)FindResource("DrinkCardStyle"),
                            Width = 180,
                            Margin = new Thickness(10),
                            Padding = new Thickness(10)
                        };

                        var stack = new StackPanel();

                      

                        // Название
                        stack.Children.Add(new TextBlock
                        {
                            Text = drink.Название,
                            FontWeight = FontWeights.Bold,
                            FontSize = 14,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            TextWrapping = TextWrapping.Wrap
                        });

                        // Цена
                        stack.Children.Add(new TextBlock
                        {
                            Text = $"{drink.Цена:0.00} ₽",
                            Foreground = Brushes.Green,
                            FontWeight = FontWeights.Bold,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(0, 5, 0, 0)
                        });

                        // Время приготовления
                        if (!string.IsNullOrEmpty(drink.ВремяПриготовления))
                        {
                            stack.Children.Add(new TextBlock
                            {
                                Text = $"⏱ {drink.ВремяПриготовления}",
                                Foreground = Brushes.Gray,
                                HorizontalAlignment = HorizontalAlignment.Center
                            });
                        }

                        card.Child = stack;
                        DrinksMenuPanel.Children.Add(card);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке меню: {ex.Message}");
            }
        }

    }

    public class DrinkOrder
    {
        public int OrderId { get; set; }
        public string DrinkName { get; set; }
        public string OrderNumber { get; set; }
        public string SpecialRequests { get; set; }
        public string TimeElapsed { get; set; }
        public string DrinkImage { get; set; }
        public string Quantity { get; set; }
        public bool IsInProgress { get; set; }
    }

    public class DrinkItem
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string PreparationTime { get; set; }
    }
}