using System.Collections.Generic;

using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Services;

using Nancy;

using Owin;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Модуль хостинга для обработки запросов приложения.
    /// </summary>
    public sealed class ApplicationOwinHostingModule : IOwinHostingModule
    {
        public ApplicationOwinHostingModule(IUserIdentityProvider userIdentityProvider,IEnumerable<IHttpService> httpModules)
        {
            ApplicationNancyModule.UserIdentityProvider = userIdentityProvider;
            ApplicationNancyModule.HttpModules = httpModules;
        }


        public OwinHostingModuleType ModuleType => OwinHostingModuleType.Application;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            builder.UseNancy();
        }


        public sealed class ApplicationNancyModule : NancyModule
        {
            public static IUserIdentityProvider UserIdentityProvider { get; set; }

            public static IEnumerable<IHttpService> HttpModules { get; set; }


            public ApplicationNancyModule()
            {
                var httpModules = HttpModules;

                if (httpModules != null)
                {
                    var httpModuleBuilder = new HttpServiceBuilder(this, UserIdentityProvider.GetCurrentUserIdentity);

                    foreach (var httpModule in httpModules)
                    {
                        httpModule.Load(httpModuleBuilder);
                    }
                }
            }
        }
    }
}