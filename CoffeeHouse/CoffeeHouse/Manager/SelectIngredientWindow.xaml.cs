using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeHouse.Manager
{
    public partial class SelectIngredientWindow : Window
    {
        public Ингредиенты SelectedIngredient { get; private set; }

        public SelectIngredientWindow()
        {
            InitializeComponent();
            LoadIngredients();
        }

        private void LoadIngredients()
        {
            try
            {
                using (var db = new CoffeeHouseEntities())
                {
                    IngredientsGrid.ItemsSource = db.Ингредиенты
                        .OrderBy(i => i.Наименование)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке ингредиентов: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            if (IngredientsGrid.SelectedItem is Ингредиенты ingredient)
            {
                // Validate the cost before accepting
                if (!decimal.TryParse(ingredient.СебестоимостьЗаЕдиницу, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    MessageBox.Show("Некорректное значение цены у выбранного ингредиента", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                SelectedIngredient = ingredient;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Выберите ингредиент", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}