using SQLite;
using System;

namespace CoffeMob
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PricePerKg { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
    }
}
