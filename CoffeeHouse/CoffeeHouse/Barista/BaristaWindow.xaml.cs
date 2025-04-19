using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CoffeeHouse.Barista
{
    public partial class BaristaWindow : Window
    {
        private Сотрудники _currentEmployee;

        public BaristaWindow(Сотрудники employee)
        {
            InitializeComponent();
            _currentEmployee = employee;
            LoadUserData();

            // Добавляем навигацию на PrepareDrinksPage при загрузке окна
            Loaded += (s, e) => NavigateToPrepareDrinksPage();
        }

        private void LoadUserData()
        {
            if (_currentEmployee != null)
            {
                UserFullNameText.Text = _currentEmployee.ФИО;
                UserRoleText.Text = "Бариста";
                UserScheduleText.Text = _currentEmployee.ГрафикРаботы;
            }
        }

        private void NavigateToPage(Page page)
        {
            MainFrame.Navigate(page);
        }

        private void NavigateToPrepareDrinksPage()
        {
            var prepareDrinksPage = new PrepareDrinksPage();
            NavigateToPage(prepareDrinksPage);
        }

        private void PrepareDrinksButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateToPrepareDrinksPage();
        }

        private void OrdersQueueButton_Click(object sender, RoutedEventArgs e)
        {
            var ordersQueuePage = new OrdersQueuePage();
            NavigateToPage(ordersQueuePage);
        }

        private void StatusChangeButton_Click(object sender, RoutedEventArgs e)
        {
            var statusChangePage = new StatusChangePage();
            NavigateToPage(statusChangePage);
        }

        private void PreparationLogButton_Click(object sender, RoutedEventArgs e)
        {
            var preparationLogPage = new PreparationLogPage();
            NavigateToPage(preparationLogPage);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
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