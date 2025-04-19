using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CoffeeHouse.Barista;
using CoffeeHouse.Manager;
using CoffeeHouse.Admin;
using CoffeeHouse.Waiter;


namespace CoffeeHouse
{
    public partial class MainWindow : Window
    {
        // Делаем свойства для доступа к UI элементам
        public string LoginText => lg.Text;
        public string PasswordText => pr.Password;
        public MainWindow()
        {
            InitializeComponent(); 
        }

        public void Next_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginText) || string.IsNullOrEmpty(PasswordText))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            using (var db = new CoffeeHouseEntities())
            {
                var employee = db.Сотрудники
                    .FirstOrDefault(u => u.Логин == LoginText && u.ХешПароля == PasswordText);

                if (employee == null)
                {
                    MessageBox.Show("Неверный логин или пароль.");
                    return;
                }

                switch (employee.Роль)
                {
                    case 1: // Администратор
                        var adminWindow = new AdminWindow(employee);
                        adminWindow.Show();
                        this.Close();
                        break;

                    case 2: // Менеджер
                        var managerWindow = new ManagerWindow(employee);
                        managerWindow.Show();
                        this.Close();
                        break;

                    case 3: // Бариста
                        var baristaWindow = new BaristaWindow(employee);
                        baristaWindow.Show();
                        this.Close();
                        break;

                    case 4: // Официант
                        var waiterWindow = new WaiterWindow(employee);
                        waiterWindow.Show();
                        this.Close();
                        break;
                    default:
                        MessageBox.Show("Неизвестная роль пользователя.");
                        break;
                }
            }
        }
    }    
}