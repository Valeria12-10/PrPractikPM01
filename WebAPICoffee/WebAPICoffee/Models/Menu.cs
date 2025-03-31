using System;
using System.Collections.Generic;

namespace WebAPICoffee.Models
{
    public class Menu
    {
        public Menu(Меню menu)
        {
            IDТовара = menu.IDТовара;
            Название = menu.Название;
            Категория = menu.Категория;
            Описание = menu.Описание;
            Цена = menu.Цена;
            Доступность = menu.Доступность;
            ВремяПриготовления = menu.ВремяПриготовления;
            ФотоТовара = menu.ФотоТовара;
            ДатаДобавления = menu.ДатаДобавления;
            ДатаИзменения = menu.ДатаИзменения;
        }

        public int IDТовара { get; set; }
        public string Название { get; set; }
        public string Категория { get; set; }
        public string Описание { get; set; }
        public decimal Цена { get; set; }
        public string Доступность { get; set; }
        public string ВремяПриготовления { get; set; }
        public string ФотоТовара { get; set; }
        public DateTime ДатаДобавления { get; set; }
        public DateTime ДатаИзменения { get; set; }
    }
}