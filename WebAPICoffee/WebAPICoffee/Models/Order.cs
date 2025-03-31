using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPICoffee.Models
{
    public class Order
    {
        public Order()
        {
            ПозицииЗаказа = new List<OrderItem>();
        }

        public int IDЗаказа { get; set; }
        public string ТипЗаказа { get; set; }
        public string Статус { get; set; }
        public DateTime ВремяСоздания { get; set; }
        public DateTime? ВремяЗавершения { get; set; }
        public decimal ИтоговаяСумма { get; set; }
        public string СпособОплаты { get; set; }
        public string КомментарийКлиента { get; set; }
        public int IDСотрудника { get; set; }
        public List<OrderItem> ПозицииЗаказа { get; set; }
    }

    public class OrderItem
    {
        public int IDПозиции { get; set; }
        public int IDЗаказа { get; set; }
        public int IDТовара { get; set; }
        public int Количество { get; set; }
        public string Модификация { get; set; }
        public decimal ЦенаНаМомент { get; set; }
    }
}