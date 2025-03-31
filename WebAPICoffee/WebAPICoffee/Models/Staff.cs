using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace WebAPICoffee.Models
{
    public class Staff
    {
      
        public Staff(Сотрудники пользователь)
        {
            IDСотрудника = пользователь.IDСотрудника;
            ФИО = пользователь.ФИО;
            Роль = пользователь.Роль;
            Логин = пользователь.Логин;
            ХешПароля = пользователь.ХешПароля;
            ГрафикРаботы = пользователь.ГрафикРаботы;
        }
        public int IDСотрудника { get; set; }
        public string ФИО { get; set; }
        public int Роль { get; set; }
        public string Логин { get; set; }
        public string ХешПароля { get; set; }
        public string ГрафикРаботы { get; set; }
    }
      
}