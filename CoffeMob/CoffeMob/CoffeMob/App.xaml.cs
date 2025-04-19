using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
namespace CoffeMob
{
    public partial class App : Application
    {
        private Database _database;
        public App()
        {
            InitializeComponent();
            // Путь к базе данных
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "products.db3");
            _database = new Database(dbPath);

            // Инициализация товаров
            InitializeProducts();

            // Установка главной страницы
            MainPage = new NavigationPage(new MainPage());


        }
        private async void InitializeProducts()
        {
            var existingProducts = await _database.GetProductsAsync();
            if (existingProducts.Count == 0) // Проверяем, есть ли уже товары
            {
                var _products = new List<Product>
                {
                   new Product { Name = "Эспрессо", PricePerKg = 120.00m, ImageUrl = "https://avatars.mds.yandex.net/get-altay/11522875/2a0000018e4aecea635f3a3b3c44a527ef0b/XXL_height" },
    new Product { Name = "Американо", PricePerKg = 130.00m, ImageUrl = "https://media.leverans.ru/product_images_inactive/spb/sushi-star/Amerikano-300x200.jpg" },
    new Product { Name = "Капучино", PricePerKg = 150.00m, ImageUrl = "https://avatars.mds.yandex.net/i?id=036feab940436a0febf01207fd09fe9edab79e6e-10108052-images-thumbs&n=13" },
    new Product { Name = "Латте", PricePerKg = 160.00m, ImageUrl = "https://avatars.mds.yandex.net/i?id=5f858591602d66fc15c99311aaa0440a_l-11846204-images-thumbs&n=13" },
    new Product { Name = "Флэт Уайт", PricePerKg = 170.00m, ImageUrl = "https://avatars.mds.yandex.net/get-altay/11902589/2a0000018e8618aef1cd86a5b593294225ee/XXXL" },
    new Product { Name = "Раф", PricePerKg = 180.00m, ImageUrl = "https://avatars.mds.yandex.net/i?id=7a3b588c940c1924c12440df2e816de3_l-4809712-images-thumbs&n=13" },
    new Product { Name = "Моккачино", PricePerKg = 190.00m, ImageUrl = "https://avatars.mds.yandex.net/i?id=64af74d2ec6873c73d47d6777f3e79d12e08728b-4233516-images-thumbs&n=13" },
    new Product { Name = "Глясе", PricePerKg = 200.00m, ImageUrl = "https://avatars.mds.yandex.net/i?id=6be69953941f33df746e2c1a6e6bb411_l-5279191-images-thumbs&n=13" },
    new Product { Name = "Фраппе", PricePerKg = 180.00m, ImageUrl = "https://avatars.mds.yandex.net/i?id=b3e98c4ed13cf42a90724818bbd04a5e_l-9071479-images-thumbs&n=13" },
    new Product { Name = "Чай черный", PricePerKg = 100.00m, ImageUrl = "https://avatars.mds.yandex.net/i?id=a82c6fd8b0dc2a3727aa369ea3c140c4_l-5289303-images-thumbs&n=13" },
    new Product { Name = "Чай зеленый", PricePerKg = 100.00m, ImageUrl = "https://avatars.mds.yandex.net/i?id=a82c6fd8b0dc2a3727aa369ea3c140c4_l-5289303-images-thumbs&n=13" },
    new Product { Name = "Какао", PricePerKg = 140.00m, ImageUrl = "https://i.pinimg.com/736x/19/d3/47/19d34710ea6d90ab8bd888ce7216213a.jpg" },
    new Product { Name = "Горячий шоколад", PricePerKg = 160.00m, ImageUrl = "" },
    new Product { Name = "Круассан", PricePerKg = 90.00m, ImageUrl = "" },
    new Product { Name = "Чизкейк", PricePerKg = 150.00m, ImageUrl = "" },
    new Product { Name = "Тирамису", PricePerKg = 170.00m, ImageUrl = "" }

                };

                foreach (var product in _products)
                {
                    await _database.SaveProductAsync(product);
                }
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
