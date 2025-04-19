using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoffeMob
{
    public partial class MainPage : ContentPage
    {
        private Database _database;
        private List<Product> _products;
        public MainPage()
        {
            InitializeComponent();
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "products.db3");
            _database = new Database(dbPath);
            LoadProducts();
        }
        private async void LoadProducts()
        {
            _products = await _database.GetProductsAsync();
            ProductListView.ItemsSource = _products;
        }

        private async void OnAddToCartClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is Product product)
            {
                var quantityEntry = (button.Parent as StackLayout).Children.FirstOrDefault(x => x is Entry) as Entry;
                if (int.TryParse(quantityEntry.Text, out int quantity) && quantity > 0)
                {
                    product.Quantity += quantity;
                    await _database.SaveProductAsync(product);
                    await DisplayAlert("Уведомление", "Товар добавлен в корзину!", "OK");
                    quantityEntry.Text = string.Empty; // Очищаем поле ввода
                }
                else
                {
                    await DisplayAlert("Ошибка", "Введите корректное количество.", "OK");
                }
            }
        }
        private async void OnProductSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Product selectedProduct)
            {
                await DisplayAlert("Выбранный товар", $"Вы выбрали: {selectedProduct.Name}", "OK");
                ((ListView)sender).SelectedItem = null; // Сброс выбора
            }
        }

        private async void OnGoToCartClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartPage());
        }
        private async void OnViewOrdersClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrdersPage());
        }
    }
}