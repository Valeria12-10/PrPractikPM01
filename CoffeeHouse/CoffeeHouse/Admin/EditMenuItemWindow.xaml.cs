using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace CoffeeHouse.Admin
{
    public partial class EditMenuItemWindow : Window
    {
        public MenuItem CurrentItem { get; private set; }
        public string[] Categories { get; } = { "Кофе", "Чай", "Десерты", "Завтраки", "Напитки", "Выпечка" };
        public string WindowTitle => CurrentItem.IDТовара == 0 ? "Добавление товара" : "Редактирование товара";
        public Visibility IdVisibility => CurrentItem.IDТовара == 0 ? Visibility.Collapsed : Visibility.Visible;

        public MenuItem MenuItem { get; internal set; }

        public EditMenuItemWindow(MenuItem item)
        {
            InitializeComponent();

            CurrentItem = item ?? new MenuItem
            {
                IDТовара = 0,
                Название = "",
                Категория = Categories[0],
                Описание = "",
                Цена = 0,
                Доступность = true,
                ВремяПриготовления = "",
                ФотоТовара = ""
            };

            if (!string.IsNullOrEmpty(CurrentItem.ФотоТовара))
            {
                try
                {
                    CurrentItem.ImageSource = new BitmapImage(new Uri(CurrentItem.ФотоТовара));
                }
                catch
                {
                    // Если не удалось загрузить изображение
                    CurrentItem.ImageSource = null;
                }
            }

            DataContext = this;
        }

        private void BrowsePhoto_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                CurrentItem.ФотоТовара = openFileDialog.FileName;
                try
                {
                    CurrentItem.ImageSource = new BitmapImage(new Uri(CurrentItem.ФотоТовара));
                }
                catch
                {
                    MessageBox.Show("Не удалось загрузить изображение", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных
            if (string.IsNullOrWhiteSpace(CurrentItem.Название))
            {
                MessageBox.Show("Введите название товара", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                NameTextBox.Focus();
                return;
            }

            if (CurrentItem.Цена <= 0)
            {
                MessageBox.Show("Цена должна быть больше нуля", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                PriceTextBox.Focus();
                return;
            }

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

    public class MenuItem
    {
        public int IDТовара { get; set; }
        public string Название { get; set; }
        public string Категория { get; set; }
        public string Описание { get; set; }
        public decimal Цена { get; set; }
        public bool Доступность { get; set; }
        public string ВремяПриготовления { get; set; }
        public string ФотоТовара { get; set; }
        public BitmapImage ImageSource { get; set; }
    }
}