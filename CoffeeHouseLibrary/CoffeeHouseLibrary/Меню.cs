//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoffeeHouseLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class Меню
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Меню()
        {
            this.ПозицииЗаказа = new HashSet<ПозицииЗаказа>();
            this.СоставБлюда = new HashSet<СоставБлюда>();
        }
    
        public int IDТовара { get; set; }
        public string Название { get; set; }
        public string Категория { get; set; }
        public string Описание { get; set; }
        public decimal Цена { get; set; }
        public string Доступность { get; set; }
        public string ВремяПриготовления { get; set; }
        public string ФотоТовара { get; set; }
        public System.DateTime ДатаДобавления { get; set; }
        public System.DateTime ДатаИзменения { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ПозицииЗаказа> ПозицииЗаказа { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<СоставБлюда> СоставБлюда { get; set; }
    }
}
