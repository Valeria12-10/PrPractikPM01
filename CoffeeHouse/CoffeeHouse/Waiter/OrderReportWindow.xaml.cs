using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CoffeeHouse.Waiter
{
    public partial class OrderReportWindow : Window
    {
        private int waiterId;
        private DateTime startDate;
        private DateTime endDate;
        private int currentWaiterId;

        public OrderReportWindow(int waiterId, DateTime startDate, DateTime endDate, int currentWaiterId)
        {
            InitializeComponent();
            this.waiterId = waiterId;
            this.startDate = startDate;
            this.endDate = endDate;
            LoadReportData();
            this.currentWaiterId = currentWaiterId;
        }

        private void LoadReportData()
        {
            using (var db = new CoffeeHouseEntities())
            {
                // Загрузка информации о сотруднике
                var waiter = db.Сотрудники.FirstOrDefault(w => w.IDСотрудника == waiterId);
                if (waiter != null)
                {
                    WaiterNameText.Text = waiter.ФИО;
                }

                // Загрузка отчетов по заказам за указанный период
                var orders = db.Заказы
                    .Where(o => o.IDСотрудника == waiterId &&
                               o.ВремяСоздания >= startDate &&
                               o.ВремяСоздания <= endDate)
                    .OrderByDescending(o => o.ВремяСоздания)
                    .ToList();

                OrdersDataGrid.ItemsSource = orders;

                // Расчет статистики
                var totalOrders = orders.Count;
                var completedOrders = orders.Count(o => o.Статус == "Завершен");
                var totalAmount = orders.Where(o => o.ИтоговаяСумма != null).Sum(o => o.ИтоговаяСумма) ?? 0;

                TotalOrdersText.Text = totalOrders.ToString();
                CompletedOrdersText.Text = completedOrders.ToString();
                TotalAmountText.Text = $"{totalAmount:N2} руб.";
                PeriodText.Text = $"{startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}";
            }
        }

        private void PrintReport_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(ReportContent, "Отчет по заказам официанта");
            }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}