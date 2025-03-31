using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPICoffee.Models;

namespace WebAPICoffee
{
    public interface IAuthService
    {
        Staff Authenticate(string email, string password);
        Staff AuthenticateWith2FA(string email, string token);
    }


    public class AuthService : IAuthService
    {
        private readonly CoffeeHouseEntities _context;

        public AuthService(CoffeeHouseEntities context)
        {
            _context = context;
        }

        public Staff Authenticate(string email, string password)
        {
            // Ищем пользователя в базе данных
            var пользователь = _context.Сотрудники
                .SingleOrDefault(u => u.Логин == email && u.ХешПароля == password);

            if (пользователь == null)
                return null;

            // Возвращаем объект Staff
            return new Staff(пользователь);
        }

        public Staff AuthenticateWith2FA(string email, string token)
        {
            // Ищем пользователя в базе данных
            var пользователь = _context.Сотрудники
                .SingleOrDefault(u => u.Логин == email);

            // Проверяем токен (здесь должна быть логика проверки 2FA)
            if (пользователь == null || token != "valid_2fa_token")
                return null;

            // Возвращаем объект Staff
            return new Staff(пользователь);
        }
    }
}