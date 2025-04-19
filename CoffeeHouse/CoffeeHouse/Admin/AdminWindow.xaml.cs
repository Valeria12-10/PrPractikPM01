using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CoffeeHouse.Barista;

namespace CoffeeHouse.Admin
{
    public partial class AdminWindow : Window
    {
        private Сотрудники _currentEmployee;

        public AdminWindow(Сотрудники employee)
        {
            InitializeComponent();
            _currentEmployee = employee;
            LoadUserData();

            // Загружаем страницу управления меню сразу при открытии окна
            MainFrame.Navigate(new MenuManagementPage());
        }

        private void LoadUserData()
        {
            if (_currentEmployee != null)
            {
                UserFullNameText.Text = _currentEmployee.ФИО;
                UserRoleText.Text = "Администратор";
                UserScheduleText.Text = _currentEmployee.ГрафикРаботы;
            }
        }

        private void SystemSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу настроек системы
            MainFrame.Navigate(new SystemSettingsPage());
        }
        private void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
        }

        private void MenuManagementButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу управления меню
            MainFrame.Navigate(new MenuManagementPage());
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу отчетов
            MainFrame.Navigate(new ReportsPage());
        }

        private void UserManagementButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу управления пользователями
            MainFrame.Navigate(new UserManagementPage());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Выход из системы
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            ((Frame)sender).NavigationService.RemoveBackEntry();
        }
    }
}