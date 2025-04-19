using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.Entity;

namespace CoffeeHouse.Manager
{
    public partial class OrdersControlPage : Page
    {
        public OrdersControlPage()
        {
            InitializeComponent();
            LoadOrders();
            InitializeDatePickers();
         
        }

        private void InitializeDatePickers()
        {
            DateFromPicker.SelectedDate = DateTime.Today.AddDays(-7);
            DateToPicker.SelectedDate = DateTime.Today;
        }



        private void LoadOrders()
        {
            using (var db = new CoffeeHouseEntities())
            {
                var query = db.Заказы.AsQueryable();

               
                if (DateFromPicker.SelectedDate != null)
                {
                    query = query.Where(o => o.ВремяСоздания >= DateFromPicker.SelectedDate);
                }

                if (DateToPicker.SelectedDate != null)
                {
                    var endDate = DateToPicker.SelectedDate.Value.AddDays(1);
                    query = query.Where(o => o.ВремяСоздания < endDate);
                }

               
                if (StatusFilterComboBox.SelectedIndex > 0)
                {
                    var selectedItem = (ComboBoxItem)StatusFilterComboBox.SelectedItem;
                    var status = selectedItem.Content.ToString();

                    
                    if (status == "в одижании") status = "в ожидании";
                    if (status == "в процессе") status = "в процессе";

                    query = query.Where(o => o.Статус == status);
                }

         
                OrdersGrid.ItemsSource = query
                    .OrderByDescending(o => o.ВремяСоздания)
                    .ToList();
            }
        }

        private void FilterOrders_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void OrderDetails_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersGrid.SelectedItem is Заказы selectedOrder)
            {
                var detailsWindow = new OrderDetailsWindow(selectedOrder);
                detailsWindow.ShowDialog();
              
                LoadOrders();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для просмотра деталей.",
                    "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}