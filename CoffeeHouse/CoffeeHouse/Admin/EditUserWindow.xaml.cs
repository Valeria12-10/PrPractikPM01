using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CoffeeHouse.Admin
{
    public partial class EditUserWindow : Window
    {
        private Сотрудники _user;

        public EditUserWindow()
        {
            InitializeComponent();
            LoadRoles();
            _user = new Сотрудники();
        }

        public EditUserWindow(Сотрудники user)
        {
            InitializeComponent();
            LoadRoles();
            _user = user;
            FullNameTextBox.Text = user.ФИО;
            LoginTextBox.Text = user.Логин;
            EmailTextBox.Text = user.Email;
            RoleComboBox.SelectedValue = user.Роль;
        }

        private void LoadRoles()
        {
            // Создаем список ролей
            var roles = new List<RoleItem>
    {
        new RoleItem { Id = 1, Name = "Администратор" },
        new RoleItem { Id = 2, Name = "Менеджер" },
        new RoleItem { Id = 3, Name = "Бариста" },
        new RoleItem { Id = 4, Name = "Официант" }
    };

            // Устанавливаем источник данных для ComboBox
            RoleComboBox.ItemsSource = roles;
            RoleComboBox.DisplayMemberPath = "Name";
            RoleComboBox.SelectedValuePath = "Id";

            // Если редактируем существующего пользователя, устанавливаем выбранную роль
            if (_user != null && _user.Роль > 0)
            {
                RoleComboBox.SelectedValue = _user.Роль;
            }
        }

        // Вспомогательный класс для хранения ролей
        public class RoleItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(LoginTextBox.Text) ||
                string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                RoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Получаем выбранную роль
                var selectedRole = (RoleItem)RoleComboBox.SelectedItem;

                using (var db = new CoffeeHouseEntities())
                {
                    if (_user.IDСотрудника == 0) // Новый пользователь
                    {
                        if (string.IsNullOrEmpty(PasswordBox.Password))
                        {
                            MessageBox.Show("Пожалуйста, укажите пароль для нового пользователя.", "Ошибка",
                                           MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        // Создаем нового сотрудника с обязательными полями
                        var newUser = new Сотрудники
                        {
                            ФИО = FullNameTextBox.Text.Trim(),
                            Логин = LoginTextBox.Text.Trim(),
                            ХешПароля = PasswordBox.Password,
                            Email = EmailTextBox.Text.Trim(),
                            Роль = selectedRole.Id,
                            // Установите значения по умолчанию для остальных обязательных полей
                            ГрафикРаботы = "Не указан" // Пример значения по умолчанию
                        };

                        db.Сотрудники.Add(newUser);
                    }
                    else // Редактирование существующего
                    {
                        var user = db.Сотрудники.FirstOrDefault(u => u.IDСотрудника == _user.IDСотрудника);
                        if (user != null)
                        {
                            user.ФИО = FullNameTextBox.Text.Trim();
                            user.Логин = LoginTextBox.Text.Trim();
                            user.Email = EmailTextBox.Text.Trim();
                            user.Роль = selectedRole.Id;

                            if (!string.IsNullOrEmpty(PasswordBox.Password))
                            {
                                user.ХешПароля = PasswordBox.Password;
                            }
                        }
                    }

                    try
                    {
                        db.SaveChanges();
                        DialogResult = true;
                        Close();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        // Собираем все сообщения об ошибках
                        var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                        var fullErrorMessage = string.Join("; ", errorMessages);
                        var exceptionMessage = $"Ошибка валидации: {fullErrorMessage}";

                        MessageBox.Show(exceptionMessage, "Ошибка сохранения",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                                       MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}