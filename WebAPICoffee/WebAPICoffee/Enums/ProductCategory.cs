using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPICoffee.Enums
{
    public enum ProductCategory
    {
        Кофе,
        Чай,
        Десерт,
        Другое
    }

    public enum OrderType
    {
        В_заведении,
        С_собой,
        Доставка
    }

    public enum OrderStatus
    {
        Новый,
        В_работе,
        Завершен,
        Отменен
    }

    public enum PaymentMethod
    {
        Наличные,
        Карта,
        Онлайн
    }

    public enum EmployeeRole
    {
        Администратор = 1,
        Менеджер = 2,
        Бариста = 3,
        Официант = 4
    }
}