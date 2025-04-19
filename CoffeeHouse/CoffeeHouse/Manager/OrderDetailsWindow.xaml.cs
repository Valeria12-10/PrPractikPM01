using System.Linq;
using System.Windows;
using System.Data.Entity;
using System.Windows.Controls;

namespace CoffeeHouse.Manager
{
    public partial class OrderDetailsWindow : Window
    {
        private Заказы _currentOrder;

        public OrderDetailsWindow(Заказы order)
        {
            InitializeComponent();
            _currentOrder = order;
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            using (var db = new CoffeeHouseEntities())
            {
                // Load the order with related data
                var fullOrder = db.Заказы
                    .Include(z => z.ПозицииЗаказа)
                    .Include(z => z.ПозицииЗаказа.Select(p => p.Меню))
                    .FirstOrDefault(o => o.IDЗаказа == _currentOrder.IDЗаказа);

                if (fullOrder != null)
                {
                    Title = $"Детали заказа №{fullOrder.Номер_заказа}";
                    DataContext = fullOrder;

                    
                    var orderItems = fullOrder.ПозицииЗаказа.Select(p => new
                    {
                        p.Меню.Название,
                        p.Количество,
                        Цена = p.ЦенаНаМомент,
                        Сумма = p.ЦенаНаМомент * decimal.Parse(p.Количество)
                    }).ToList();

                    OrderItemsGrid.ItemsSource = orderItems;
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new CoffeeHouseEntities())
            {
                var orderToUpdate = db.Заказы.Find(_currentOrder.IDЗаказа);
                if (orderToUpdate != null)
                {
                    // Get the actual status text from the selected ComboBoxItem
                    if (StatusComboBox.SelectedItem is ComboBoxItem selectedItem)
                    {
                        orderToUpdate.Статус = selectedItem.Content.ToString();
                    }

                    orderToUpdate.КомментарийКлиента = ((Заказы)DataContext).КомментарийКлиента;

                    db.SaveChanges();
                    MessageBox.Show("Изменения сохранены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}