using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CoffeeHouse.Waiter;

namespace CoffeeHouse.Manager
{
    public partial class SupplierOrdersPage : Page
    {
        private CoffeeHouseEntities db = new CoffeeHouseEntities();

        public SupplierOrdersPage()
        {
            InitializeComponent();
            LoadSupplierOrders();
        }

        private void LoadSupplierOrders()
        {
            try
            {
                var orders = db.Заказы
                    .Where(o => o.ТипЗаказа == "Поставщику")
                    .AsEnumerable() // Switch to LINQ to Objects
                    .Select(o => new SupplierOrder
                    {
                        IDЗаявки = o.IDЗаказа,
                        Поставщик = o.КомментарийКлиента,
                        ДатаСоздания = o.ВремяСоздания,
                        Статус = o.Статус,
                        Сумма = o.ИтоговаяСумма ?? 0,
                        СрокПоставки = o.ВремяЗавершения ?? DateTime.Now.AddDays(7) // Now this will work
                    }).ToList();

                SupplierOrdersGrid.ItemsSource = orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заявок: {ex.Message}");
            }
        }

        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new CreateSupplierOrderWindow();
            if (createWindow.ShowDialog() == true)
            {
                LoadSupplierOrders();
            }
        }

        private void SendOrder_Click(object sender, RoutedEventArgs e)
        {
            if (SupplierOrdersGrid.SelectedItem is SupplierOrder selectedOrder)
            {
                try
                {
                    var order = db.Заказы.Find(selectedOrder.IDЗаявки);
                    if (order != null)
                    {
                        order.Статус = "Отправлено поставщику";
                        db.SaveChanges();
                        LoadSupplierOrders();
                        MessageBox.Show("Заявка успешно отправлена поставщику");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при отправке заявки: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заявку для отправки");
            }
        }

        private void SupplierOrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SupplierOrdersGrid.SelectedItem is SupplierOrder selectedOrder)
            {
                LoadOrderItems(selectedOrder.IDЗаявки);
            }
        }

        private void LoadOrderItems(int orderId)
        {
            try
            {
                var items = db.ПозицииЗаказа
                    .Where(p => p.IDЗаказа == orderId)
                    .Join(db.Ингредиенты,
                        p => p.IDТовара,
                        i => i.IDИнгредиента,
                        (p, i) => new OrderItem
                        {
                            Название = i.Наименование,
                            Количество = p.Количество,
                            ЕдиницаИзмерения = i.ЕдиницаИзмерения,
                            Цена = p.ЦенаНаМомент,
                            Сумма = p.ЦенаНаМомент * decimal.Parse(p.Количество)
                        }).ToList();

                OrderItemsGrid.ItemsSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке позиций заявки: {ex.Message}");
            }
        }
    }
    public class SupplierOrder
    {
        public int IDЗаявки { get; set; }
        public string Поставщик { get; set; }
        public DateTime ДатаСоздания { get; set; }
        public string Статус { get; set; }
        public decimal Сумма { get; set; }
        public DateTime СрокПоставки { get; set; }
    }

    public class OrderItem
    {
        public string Название { get; set; }
        public string Количество { get; set; }
        public string ЕдиницаИзмерения { get; set; }
        public decimal Цена { get; set; }
        public decimal Сумма { get; set; }
    }
}