using System;
using System.Linq;

namespace CoffeeHouseLibrary
{
    public class CoffeeHouseManager
    {
        private readonly CoffeeHouseEntities _context;

        public CoffeeHouseManager(CoffeeHouseEntities context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        // 1. Подсчет общего количества заказов (с возможностью фильтрации по дате)
        public int GetTotalOrdersCount(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Заказы.AsQueryable();

            if (startDate.HasValue)
                query = query.Where(z => z.ВремяСоздания >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(z => z.ВремяСоздания <= endDate.Value);

            return query.Count();
        }

        // 2. Подсчет суммы стоимости заказов за конкретный месяц

        public decimal GetMonthlyRevenue(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            return _context.Заказы
                .Where(z => z.ВремяСоздания >= startDate && z.ВремяСоздания <= endDate)
                .Sum(z => z.ИтоговаяСумма ?? 0m);
        }

        // 3. Подсчет количества заказов определенного типа за определенный период
        public int GetOrdersCountByType(string orderType, DateTime startDate, DateTime endDate)
        {
            return _context.Заказы
                .Count(z => z.ТипЗаказа == orderType && z.ВремяСоздания >= startDate && z.ВремяСоздания <= endDate);
        }

        // 4. Подсчет количества проданных товаров определенной категории за определенный период
        public int GetSoldItemsByCategory(string category, DateTime startDate, DateTime endDate)
        {
            var items = _context.ПозицииЗаказа
                .Where(pz => pz.Заказы.ВремяСоздания >= startDate &&
                            pz.Заказы.ВремяСоздания <= endDate &&
                            pz.Меню.Категория == category)
                .ToList(); // Материализуем запрос

            return items.Sum(pz => SafeConvertToInt(pz.Количество));
        }

        // 5. Подсчет среднего времени приготовления заказов
        public TimeSpan GetAverageOrderPreparationTime()
        {
            var completedOrders = _context.Заказы
                .Where(z => z.ВремяЗавершения != null)
                .ToList(); // Материализуем запрос

            if (!completedOrders.Any())
                return TimeSpan.Zero;

            var averageTicks = completedOrders
                .Average(z => (z.ВремяЗавершения.Value - z.ВремяСоздания).Ticks);

            return TimeSpan.FromTicks((long)averageTicks);
        }

        // 6. Подсчет остатков ингредиентов ниже минимального запаса
        public int GetLowStockIngredientsCount()
        {
            var ingredients = _context.Ингредиенты.ToList(); // Материализуем запрос
            return ingredients.Count(i => SafeConvertToInt(i.ТекущийОстаток) < SafeConvertToInt(i.МинимальныйЗапас));
        }

        // Вспомогательный метод для безопасного преобразования строки в целое число
        private int SafeConvertToInt(string value)
        {
            if (int.TryParse(value, out int result))
            {
                return result;
            }
            return 0;
        }
    }
}