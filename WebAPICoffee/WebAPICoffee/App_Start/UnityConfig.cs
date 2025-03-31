using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using Unity;
using Unity.Lifetime;
using WebAPICoffee.Controllers;

namespace WebAPICoffee.App_Start
{
    public static class UnityConfig
    {
        // Единственное определение для container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        // Единственное определение для Container
        public static IUnityContainer Container => container.Value;

        public static void RegisterTypes(IUnityContainer container)
        {
            // Регистрация сервисов
            container.RegisterType<IAuthService, AuthService>(new HierarchicalLifetimeManager());

            // Регистрация контроллеров
           container.RegisterType<AuthController>();
        }
    }
}