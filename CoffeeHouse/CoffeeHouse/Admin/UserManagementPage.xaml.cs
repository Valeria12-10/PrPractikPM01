using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CoffeeHouse.Admin;

namespace CoffeeHouse.Admin
{
    public partial class UserManagementPage : Page
    {
        private CoffeeHouseEntities _context = new CoffeeHouseEntities();

        public UserManagementPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            using (var db = new CoffeeHouseEntities())
            {
                UsersGrid.ItemsSource = db.Сотрудники
                    .Select(s => new UserDisplay
                    {
                        IDСотрудника = s.IDСотрудника,
                        ФИО = s.ФИО,
                        Логин = s.Логин,
                        Email = s.Email,
                        Роль = s.Роль == 1 ? "Администратор" :
                               s.Роль == 2 ? "Менеджер" :
                               s.Роль == 3 ? "Бариста" :
                               s.Роль == 4 ? "Официант" : "Неизвестно"
                    })
                    .ToList();
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string searchText = SearchBox.Text.ToLower();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    LoadUsers();
                }
                else
                {
                    UsersGrid.ItemsSource = _context.Сотрудники
                        .Where(u => u.Логин.ToLower().Contains(searchText) ||
                                    u.ФИО.ToLower().Contains(searchText) ||
                                    u.Email.ToLower().Contains(searchText))
                        .Select(s => new UserDisplay
                        {
                            IDСотрудника = s.IDСотрудника,
                            ФИО = s.ФИО,
                            Логин = s.Логин,
                            Email = s.Email,
                            Роль = _context.Роль.FirstOrDefault(r => r.IDРоли == s.Роль).Название
                        })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditUserWindow();
            if (editWindow.ShowDialog() == true)
            {
                _context = new CoffeeHouseEntities(); // Обновляем контекст
                LoadUsers();
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя для редактирования.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedUser = (UserDisplay)UsersGrid.SelectedItem;
            var user = _context.Сотрудники.Find(selectedUser.IDСотрудника);

            if (user != null)
            {
                var editWindow = new EditUserWindow(user);
                if (editWindow.ShowDialog() == true)
                {
                    _context.SaveChanges();
                    _context = new CoffeeHouseEntities(); // Обновляем контекст
                    LoadUsers();
                }
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя для удаления.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Удалить выбранного пользователя?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedUser = (UserDisplay)UsersGrid.SelectedItem;
                var user = _context.Сотрудники.Find(selectedUser.IDСотрудника);

                if (user != null)
                {
                    _context.Сотрудники.Remove(user);
                    _context.SaveChanges();
                    LoadUsers();
                }
            }
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            if (UsersGrid.SelectedItem == null)
            {
                MessageBox.Show("Выберите пользователя для сброса пароля.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedUser = (UserDisplay)UsersGrid.SelectedItem;
            var user = _context.Сотрудники.Find(selectedUser.IDСотрудника);

            if (user != null)
            {
                string newPassword = GenerateRandomPassword(8);
                user.ХешПароля = newPassword;
                _context.SaveChanges();

                MessageBox.Show($"Пароль сброшен. Новый пароль: {newPassword}", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var random = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    public class UserDisplay
    {
        public int IDСотрудника { get; set; }
        public string ФИО { get; set; }
        public string Логин { get; set; }
        public string Email { get; set; }
        public string Роль { get; set; }
    }
}