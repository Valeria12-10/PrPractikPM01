using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CoffeMob
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersPage : ContentPage
    {
        private Database _database;
        public OrdersPage()
        {
            InitializeComponent();
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "products.db3");
            _database = new Database(dbPath);
            LoadOrders();
        }
        private async void LoadOrders()
        {
            var orders = await _database.GetOrdersAsync();
            OrdersListView.ItemsSource = orders; // Устанавливаем источник данных для ListView
        }
    }
}