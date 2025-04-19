using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CoffeMob
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        private Database _database;
        private List<Product> _cartProducts;

        public CartPage()
        {
            InitializeComponent();
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "products.db3");
            _database = new Database(dbPath);
            LoadCart();
        }

        private async void LoadCart()
        {
            _cartProducts = await _database.GetProductsAsync();
            var cartItems = _cartProducts
                .Where(p => p.Quantity > 0)
                .Select(p => new CartItem
                {
                    Name = p.Name,
                    Quantity = p.Quantity,
                    TotalPrice = p.PricePerKg * p.Quantity
                })
                .ToList();

            CartListView.ItemsSource = cartItems;
            UpdateTotalAmount(cartItems);
            UpdateTotalQuantity(cartItems);
        }

        private void UpdateTotalAmount(List<CartItem> cartItems)
        {
            var totalAmount = cartItems.Sum(item => item.TotalPrice);
            TotalAmountLabel.Text = $"Общая сумма: {totalAmount:C}";
        }

        private void UpdateTotalQuantity(List<CartItem> cartItems)
        {
            var totalQuantity = cartItems.Sum(item => item.Quantity);
            TotalQuantityLabel.Text = $"Общее количество товаров: {totalQuantity}";

            if (totalQuantity > 0)
            {
                // Уведомление о том, что корзина заполнена
                DisplayAlert("Уведомление", "Корзина заполнена, пора оформить заказ!", "OK");
            }
        }

        private async void OnRemoveFromCartClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is CartItem cartItem)
            {
                // Найти продукт в корзине
                var product = _cartProducts.FirstOrDefault(p => p.Name == cartItem.Name);
                if (product != null)
                {
                    // Удаляем товар из корзины
                    product.Quantity = 0; // Устанавливаем количество в 0
                    await _database.SaveProductAsync(product); // Обновляем продукт в базе данных

                    // Обновляем корзину
                    LoadCart();
                    await DisplayAlert("Уведомление", "Товар удален из корзины!", "OK");
                }
            }
        }

        private async void OnCheckoutClicked(object sender, EventArgs e)
        {
            var cartItems = await _database.GetProductsAsync();
            var orders = new List<Order>();

            foreach (var product in cartItems.Where(p => p.Quantity > 0))
            {
                var order = new Order
                {
                    ProductName = product.Name,
                    Quantity = product.Quantity,
                    TotalPrice = product.PricePerKg * product.Quantity,
                    OrderDate = DateTime.Now
                };

                orders.Add(order);
                await _database.SaveOrderAsync(order); // Сохраняем заказ в базе данных

                // Очищаем количество товара в корзине
                product.Quantity = 0;
                await _database.SaveProductAsync(product); // Обновляем продукт в базе данных
            }

            await DisplayAlert("Уведомление", "Заказ оформлен!", "OK");
            LoadCart(); // Обновляем корзину
        }
    }
}