using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeHouse.Manager
{
    public partial class CreateSupplierOrderWindow : Window
    {
        private CoffeeHouseEntities db = new CoffeeHouseEntities();
        private List<OrderItem> orderItems = new List<OrderItem>();
        private int currentEmployeeId;

        public CreateSupplierOrderWindow()
        {
            InitializeComponent();
            LoadCurrentEmployee();
            DeliveryDatePicker.SelectedDate = DateTime.Now.AddDays(7);
            UpdateTotalAmount();
        }

        private void LoadCurrentEmployee()
        {
            // In a real app, you would get this from authentication
            currentEmployeeId = 1; // Example employee ID
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            var selectWindow = new SelectIngredientWindow();
            if (selectWindow.ShowDialog() == true && selectWindow.SelectedIngredient != null)
            {
                var ingredient = selectWindow.SelectedIngredient;

                try
                {
                    // Handle potential decimal separator issues
                    string costValue = ingredient.СебестоимостьЗаЕдиницу?
                        .Replace(",", ".")  // Replace comma with dot
                        .Trim();

                    if (!decimal.TryParse(costValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price))
                    {
                        MessageBox.Show("Некорректное значение цены в базе данных", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var item = new OrderItem
                    {
                        Название = ingredient.Наименование,
                        Количество = "1",
                        ЕдиницаИзмерения = ingredient.ЕдиницаИзмерения,
                        Цена = price,
                        Сумма = price
                    };

                    orderItems.Add(item);
                    ItemsGrid.ItemsSource = null;
                    ItemsGrid.ItemsSource = orderItems;
                    UpdateTotalAmount();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении ингредиента: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (ItemsGrid.SelectedItem is OrderItem selectedItem)
            {
                orderItems.Remove(selectedItem);
                ItemsGrid.ItemsSource = null;
                ItemsGrid.ItemsSource = orderItems;
                UpdateTotalAmount();
            }
        }

        private void UpdateTotalAmount()
        {
            decimal total = orderItems.Sum(item => item.Сумма);
            TotalAmountText.Text = total.ToString("C");
        }



        private void SendToSupplier_Click(object sender, RoutedEventArgs e)
        {
            if (orderItems.Count == 0)
            {
                MessageBox.Show("Добавьте хотя бы один товар в заявку");
                return;
            }

            if (SupplierComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите поставщика");
                return;
            }

            if (DeliveryDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Укажите срок поставки");
                return;
            }

            SaveOrder("Отправлено поставщику");
        }

        private void SaveOrder(string status)
        {
            try
            {
                var supplier = (SupplierComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                var deliveryDate = DeliveryDatePicker.SelectedDate ?? DateTime.Now.AddDays(7);
                var comment = CommentTextBox.Text;
                var totalAmount = orderItems.Sum(item => item.Сумма);

                var newOrder = new Заказы
                {
                    ТипЗаказа = "Поставщику",
                    Статус = status,
                    Номер_заказа = GenerateOrderNumber(),
                    ВремяСоздания = DateTime.Now,
                    ВремяЗавершения = deliveryDate,
                    ИтоговаяСумма = totalAmount,
                    КомментарийКлиента = supplier,
                    IDСотрудника = currentEmployeeId
                };

                db.Заказы.Add(newOrder);
                db.SaveChanges();

                foreach (var item in orderItems)
                {
                    var ingredient = db.Ингредиенты.FirstOrDefault(i => i.Наименование == item.Название);
                    if (ingredient != null)
                    {
                        var orderItem = new ПозицииЗаказа
                        {
                            IDЗаказа = newOrder.IDЗаказа,
                            IDТовара = ingredient.IDИнгредиента,
                            Количество = item.Количество,
                            ЦенаНаМомент = item.Цена
                        };
                        db.ПозицииЗаказа.Add(orderItem);
                    }
                }

                db.SaveChanges();
                MessageBox.Show("Заявка успешно сохранена");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заявки: {ex.Message}");
            }
        }

        private string GenerateOrderNumber()
        {
            return $"SUP-{DateTime.Now:yyyyMMdd-HHmmss}";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}