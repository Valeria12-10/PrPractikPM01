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
    
    public partial class Сотрудники
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Сотрудники()
        {
            this.Заказы = new HashSet<Заказы>();
            this.ОтчетПерсонала = new HashSet<ОтчетПерсонала>();
        }
    
        public int IDСотрудника { get; set; }
        public string ФИО { get; set; }
        public string Логин { get; set; }
        public string ХешПароля { get; set; }
        public string Email { get; set; }
        public string ГрафикРаботы { get; set; }
        public Nullable<int> Роль { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Заказы> Заказы { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ОтчетПерсонала> ОтчетПерсонала { get; set; }
        public virtual Роль Роль1 { get; set; }
    }
}
