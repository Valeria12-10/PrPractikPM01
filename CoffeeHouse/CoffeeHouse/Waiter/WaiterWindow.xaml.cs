using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CoffeeHouse.Waiter
{
    public partial class WaiterWindow : Window
    {
        private List<Меню> _menuItems;
        private List<ПозицииЗаказа> _currentOrderItems = new List<ПозицииЗаказа>();
        private int _currentWaiterId;
        private Сотрудники employee;
        private int waiterId;
        private string waiterName;
       public WaiterWindow(Сотрудники employee)
        {
            InitializeComponent();
            _currentWaiterId = waiterId;
            WaiterNameText.Text = waiterName;
            LoadMenuItems();
            LoadTables();
            UpdateOrderGrid();
            this.employee = employee;
            if (employee == null || employee.IDСотрудника == 0)
            {
                MessageBox.Show("Неверные данные сотрудника!", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
                return;
            }

           
            _currentWaiterId = employee.IDСотрудника;
        }


        private void LoadTables()
        {
            TableNumberCombo.Items.Clear();
            for (int i = 1; i <= 20; i++)
            {
                TableNumberCombo.Items.Add(new ComboBoxItem { Content = i.ToString() });
            }
            TableNumberCombo.SelectedIndex = 0;
        }

        private void LoadMenuItems()
        {
            using (var db = new CoffeeHouseEntities())
            {
                _menuItems = db.Меню.Where(m => m.Доступность == "Доступен").ToList();
                DisplayMenuItems(_menuItems);
            }
        }

        private void DisplayMenuItems(List<Меню> items)
        {
            MenuItemsPanel.Children.Clear();

            foreach (var item in items)
            {
                var card = new Border
                {
                    Style = (Style)FindResource("MenuItemCardStyle"),
                    ToolTip = item.Описание
                };

                var stackPanel = new StackPanel();

            
                stackPanel.Children.Add(new TextBlock
                {
                    Text = item.Название,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 5, 0, 0)
                });

                stackPanel.Children.Add(new TextBlock
                {
                    Text = $"{item.Цена} руб."
                });

                var addButton = new Button
                {
                    Content = "Добавить",
                    Style = (Style)FindResource("MenuButtonStyle"),
                    Tag = item.IDТовара
                };
                addButton.Click += AddToOrder_Click;
                stackPanel.Children.Add(addButton);

               

                card.Child = stackPanel;
                MenuItemsPanel.Children.Add(card);
            }
        }

        private void ShowIngredients_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int productId = (int)button.Tag;

            using (var db = new CoffeeHouseEntities())
            {
                var ingredients = db.СоставБлюда
                    .Where(c => c.IDТовара == productId)
                    .Join(db.Ингредиенты,
                        c => c.IDИнгредиента,
                        i => i.IDИнгредиента,
                        (c, i) => new { Ингредиент = i.Наименование, Количество = c.Количество, Единица = i.ЕдиницаИзмерения })
                    .ToList();

                string message = "Ингредиенты:\n";
                foreach (var ing in ingredients)
                {
                    message += $"{ing.Ингредиент} - {ing.Количество} {ing.Единица}\n";
                }

                MessageBox.Show(message, "Состав блюда", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void AddToOrder_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int productId = (int)button.Tag;

            using (var db = new CoffeeHouseEntities())
            {
                var product = db.Меню.FirstOrDefault(p => p.IDТовара == productId);
                if (product == null) return;

                var existingItem = _currentOrderItems.FirstOrDefault(i => i.IDТовара == productId);
                if (existingItem != null)
                {
                    existingItem.Количество = (int.Parse(existingItem.Количество) + 1).ToString();
                }
                else
                {
                    _currentOrderItems.Add(new ПозицииЗаказа
                    {
                        IDТовара = productId,
                        Количество = "1",
                        ЦенаНаМомент = product.Цена,
                        Модификация = "Стандарт"
                    });
                }

                UpdateOrderGrid();
            }
        }

        private void UpdateOrderGrid()
        {
            CurrentOrderGrid.ItemsSource = null;

            var orderItems = _currentOrderItems.Select(item => new
            {
                item.IDТовара,
                Название = GetProductName(item.IDТовара),
                item.Количество,
                Цена = item.ЦенаНаМомент,
                Сумма = item.ЦенаНаМомент * int.Parse(item.Количество)
            }).ToList();

            CurrentOrderGrid.ItemsSource = orderItems;
            CalculateTotal();
        }

        private string GetProductName(int productId)
        {
            using (var db = new CoffeeHouseEntities())
            {
                return db.Меню.FirstOrDefault(p => p.IDТовара == productId)?.Название ?? "Неизвестный товар";
            }
        }

        private void CalculateTotal()
        {
            decimal total = _currentOrderItems.Sum(item => item.ЦенаНаМомент * int.Parse(item.Количество));
            TotalAmountText.Text = $"{total} руб.";
        }

        private void ClearOrder_Click(object sender, RoutedEventArgs e)
        {
            _currentOrderItems.Clear();
            UpdateOrderGrid();
        }

        private void SubmitOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_currentOrderItems.Count == 0)
            {
                MessageBox.Show("Заказ пуст!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (TableNumberCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите номер столика!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var tableNumber = ((ComboBoxItem)TableNumberCombo.SelectedItem).Content.ToString();

            try
            {
                using (var db = new CoffeeHouseEntities())
                {
                    // Проверяем существование сотрудника
                    var employeeExists = db.Сотрудники.Any(s => s.IDСотрудника == _currentWaiterId);
                    if (!employeeExists)
                    {
                        MessageBox.Show("Ошибка: указанный сотрудник не найден в системе!", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var newOrder = new Заказы
                    {
                        ТипЗаказа = "В заведении",
                        Статус = "Принят",
                        Номер_заказа = $"T{tableNumber}-{DateTime.Now:HHmmss}",
                        ВремяСоздания = DateTime.Now,
                        ИтоговаяСумма = _currentOrderItems.Sum(item => item.ЦенаНаМомент * int.Parse(item.Количество)),
                        IDСотрудника = _currentWaiterId
                    };

                    db.Заказы.Add(newOrder);
                    db.SaveChanges();

                    foreach (var item in _currentOrderItems)
                    {
                        var orderItem = new ПозицииЗаказа
                        {
                            IDЗаказа = newOrder.IDЗаказа,
                            IDТовара = item.IDТовара,
                            Количество = item.Количество,
                            ЦенаНаМомент = item.ЦенаНаМомент,
                            Модификация = item.Модификация
                        };
                        db.ПозицииЗаказа.Add(orderItem);
                    }

                    db.SaveChanges();

                    MessageBox.Show($"Заказ #{newOrder.Номер_заказа} успешно создан!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    _currentOrderItems.Clear();
                    UpdateOrderGrid();
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                MessageBox.Show($"Ошибка базы данных: {sqlEx.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заказа: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_currentOrderItems.Count > 0)
            {
                var result = MessageBox.Show("Текущий заказ будет очищен. Продолжить?", "Новый заказ", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No) return;
            }

            _currentOrderItems.Clear();
            UpdateOrderGrid();
        }

        private void ShowReport_Click(object sender, RoutedEventArgs e)
        {
            // Устанавливаем период отчета - например, за последнюю неделю
            DateTime endDate = DateTime.Now;
            DateTime startDate = endDate.AddDays(-7);

            var reportWindow = new OrderReportWindow(
                waiterId: _currentWaiterId,
                startDate: startDate,
                endDate: endDate,
                currentWaiterId: _currentWaiterId
            );
            reportWindow.ShowDialog();
        }

        private void CategoryFilterCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterMenuItems();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterMenuItems();
        }

        private void FilterMenuItems()
        {
            var filtered = _menuItems.AsQueryable();

            if (CategoryFilterCombo.SelectedIndex > 0)
            {
                string category = ((ComboBoxItem)CategoryFilterCombo.SelectedItem).Content.ToString();
                filtered = filtered.Where(m => m.Категория == category);
            }

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text) && SearchTextBox.Text != "Поиск...")
            {
                string searchText = SearchTextBox.Text.ToLower();
                filtered = filtered.Where(m => m.Название.ToLower().Contains(searchText) ||
                                      (m.Описание != null && m.Описание.ToLower().Contains(searchText)));
            }

            DisplayMenuItems(filtered.ToList());
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Поиск...")
            {
                SearchTextBox.Text = "";
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchTextBox.Text = "Поиск...";
            }
        }
    }
}