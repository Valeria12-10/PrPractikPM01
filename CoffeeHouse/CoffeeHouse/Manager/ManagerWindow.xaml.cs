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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CoffeeHouse.Admin;

namespace CoffeeHouse.Manager
{
    public partial class ManagerWindow : Window
    {
        private Сотрудники _currentEmployee;

        public ManagerWindow(Сотрудники employee)
        {
            InitializeComponent();
            _currentEmployee = employee;
            LoadUserData();

            // Сразу открываем страницу контроля заказов
            MainFrame.Navigate(new OrdersControlPage());
        }

        private void LoadUserData()
        {
            if (_currentEmployee != null)
            {
                UserFullNameText.Text = _currentEmployee.ФИО;
                UserRoleText.Text = "Менеджер";
                UserScheduleText.Text = _currentEmployee.ГрафикРаботы;
            }
        }

        private void OrdersControlButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу контроля заказов
            MainFrame.Navigate(new OrdersControlPage());
        }

        private void SupplierOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу заявок поставщикам
            MainFrame.Navigate(new SupplierOrdersPage());
        }

        private void MenuManagementButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу управления меню
            MainFrame.Navigate(new MenuManagementPage());
        }

        private void ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу отчетов
            MainFrame.Navigate(new ManagerReportsPage());
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Выход из системы
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            ((Frame)sender).NavigationService.RemoveBackEntry();
        }
    }
}