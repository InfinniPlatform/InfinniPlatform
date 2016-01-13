using System.Collections.Generic;

using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Services;

using Nancy;

using Owin;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// ћодуль хостинга дл€ обработки запросов приложени€.
    /// </summary>
    public sealed class ApplicationOwinHostingModule : IOwinHostingModule
    {
        public ApplicationOwinHostingModule(IEnumerable<IHttpService> httpModules)
        {
            ApplicationNancyModule.HttpModules = httpModules;
        }


        public OwinHostingModuleType ModuleType => OwinHostingModuleType.Application;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            builder.UseNancy();
        }


        public sealed class ApplicationNancyModule : NancyModule
        {
            public static IEnumerable<IHttpService> HttpModules { get; set; }


            public ApplicationNancyModule()
            {
                var httpModules = HttpModules;

                if (httpModules != null)
                {
                    var httpModuleBuilder = new HttpServiceBuilder(this);

                    foreach (var httpModule in httpModules)
                    {
                        httpModule.Load(httpModuleBuilder);
                    }
                }
            }
        }
    }
}