using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeHouse.Manager
{
    public partial class AddSupplierOrderItemWindow : Window
    {
        public SupplierOrderItem OrderItem { get; private set; }

        public AddSupplierOrderItemWindow()
        {
            InitializeComponent();
            LoadProducts();
            CalculateAmount();

            // Подписка на события изменения количества и цены
            QuantityTextBox.TextChanged += (s, e) => CalculateAmount();
            PriceTextBox.TextChanged += (s, e) => CalculateAmount();
        }

        private void LoadProducts()
        {
            using (var db = new CoffeeHouseEntities())
            {
                var ingredients = db.Ингредиенты.ToList();
                foreach (var ingredient in ingredients)
                {
                    var item = new ComboBoxItem
                    {
                        Content = ingredient.Наименование,
                        Tag = ingredient
                    };
                    ProductComboBox.Items.Add(item);
                }

                if (ProductComboBox.Items.Count > 0)
                    ProductComboBox.SelectedIndex = 0;
            }
        }

        private void CalculateAmount()
        {
            if (decimal.TryParse(QuantityTextBox.Text, out decimal quantity) &&
                decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                decimal amount = quantity * price;
                AmountTextBlock.Text = amount.ToString("C");
            }
            else
            {
                AmountTextBlock.Text = "0";
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!decimal.TryParse(QuantityTextBox.Text, out decimal quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Введите корректную цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ProductComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите товар", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedItem = (ComboBoxItem)ProductComboBox.SelectedItem;
            var ingredient = (Ингредиенты)selectedItem.Tag;

            OrderItem = new SupplierOrderItem
            {
                Название = ingredient.Наименование,
                Количество = quantity,
                ЕдиницаИзмерения = ingredient.ЕдиницаИзмерения,
                Цена = price,
                Сумма = quantity * price
            };

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ProductComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductComboBox.SelectedItem is ComboBoxItem selectedItem &&
                selectedItem.Tag is Ингредиенты ingredient)
            {
                UnitTextBlock.Text = ingredient.ЕдиницаИзмерения;
                PriceTextBox.Text = ingredient.СебестоимостьЗаЕдиницу;
            }
        }
    }
    public class SupplierOrderItem
    {
        public string Название { get; set; }
        public decimal Количество { get; set; }
        public string ЕдиницаИзмерения { get; set; }
        public decimal Цена { get; set; }
        public decimal Сумма { get; set; }
    }
}