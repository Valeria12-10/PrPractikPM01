using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeHouse.Admin
{
    public partial class MenuManagementPage : Page
    {
        private CoffeeHouseEntities _db;

        public MenuManagementPage()
        {
            InitializeComponent();
            _db = new CoffeeHouseEntities();
            LoadMenuItems();
        }

        private void LoadMenuItems()
        {
            try
            {
                MenuItemsGrid.ItemsSource = _db.Меню.ToList().Select(item => new MenuDisplayItem
                {
                    IDТовара = item.IDТовара,
                    Название = item.Название,
                    Категория = item.Категория,
                    Цена = item.Цена,
                    Доступность = item.Доступность == "доступен",
                    ВремяПриготовления = item.ВремяПриготовления
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddItem_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditMenuItemWindow(null);
            if (editWindow.ShowDialog() == true)
            {
                try
                {
                    var newItem = new Меню
                    {
                        Название = editWindow.CurrentItem.Название,
                        Категория = editWindow.CurrentItem.Категория,
                        Описание = editWindow.CurrentItem.Описание,
                        Цена = editWindow.CurrentItem.Цена,
                        Доступность = editWindow.CurrentItem.Доступность ? "доступен" : "не доступен",
                        ВремяПриготовления = editWindow.CurrentItem.ВремяПриготовления,
                        ФотоТовара = editWindow.CurrentItem.ФотоТовара,
                        ДатаДобавления = DateTime.Now,
                        ДатаИзменения = DateTime.Now
                    };

                    _db.Меню.Add(newItem);
                    _db.SaveChanges();
                    LoadMenuItems();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении товара: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            if (MenuItemsGrid.SelectedItem is MenuDisplayItem selectedItem)
            {
                var dbItem = _db.Меню.Find(selectedItem.IDТовара);
                if (dbItem != null)
                {
                    var itemToEdit = new MenuItem
                    {
                        IDТовара = dbItem.IDТовара,
                        Название = dbItem.Название,
                        Категория = dbItem.Категория,
                        Описание = dbItem.Описание,
                        Цена = dbItem.Цена,
                        Доступность = dbItem.Доступность == "доступен",
                        ВремяПриготовления = dbItem.ВремяПриготовления,
                        ФотоТовара = dbItem.ФотоТовара
                    };

                    var editWindow = new EditMenuItemWindow(itemToEdit);
                    if (editWindow.ShowDialog() == true)
                    {
                        try
                        {
                            dbItem.Название = editWindow.CurrentItem.Название;
                            dbItem.Категория = editWindow.CurrentItem.Категория;
                            dbItem.Описание = editWindow.CurrentItem.Описание;
                            dbItem.Цена = editWindow.CurrentItem.Цена;
                            dbItem.Доступность = editWindow.CurrentItem.Доступность ? "доступен" : "не доступен";
                            dbItem.ВремяПриготовления = editWindow.CurrentItem.ВремяПриготовления;
                            dbItem.ФотоТовара = editWindow.CurrentItem.ФотоТовара;
                            dbItem.ДатаИзменения = DateTime.Now;

                            _db.SaveChanges();
                            LoadMenuItems();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при редактировании товара: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (MenuItemsGrid.SelectedItem is MenuDisplayItem selectedItem)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить товар '{selectedItem.Название}'?",
                    "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var itemToDelete = _db.Меню.Find(selectedItem.IDТовара);
                        if (itemToDelete != null)
                        {
                            _db.Меню.Remove(itemToDelete);
                            _db.SaveChanges();
                            LoadMenuItems();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении товара: {ex.Message}", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
    public class MenuDisplayItem
    {
        public int IDТовара { get; set; }
        public string Название { get; set; }
        public string Категория { get; set; }
        public decimal Цена { get; set; }
        public bool Доступность { get; set; }
        public string ВремяПриготовления { get; set; }
    }
}