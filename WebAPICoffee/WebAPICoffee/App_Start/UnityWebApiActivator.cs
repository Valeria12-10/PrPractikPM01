using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebAPICoffee.App_Start;
using Unity.AspNet.WebApi;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebAPICoffee.UnityWebApiActivator), nameof(WebAPICoffee.UnityWebApiActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(WebAPICoffee.UnityWebApiActivator), nameof(WebAPICoffee.UnityWebApiActivator.Shutdown))]

namespace WebAPICoffee
{

        public static class UnityWebApiActivator
        {

            public static void Start()
            {

                var resolver = new UnityDependencyResolver(UnityConfig.Container);

                GlobalConfiguration.Configuration.DependencyResolver = resolver;
            }

            public static void Shutdown()
            {
                UnityConfig.Container.Dispose();
            }
        }
    }