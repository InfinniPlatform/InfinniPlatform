using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Logging;

using Microsoft.AspNet.SignalR;

using Owin;

namespace InfinniPlatform.PushNotification.Owin
{
    /// <summary>
    /// Модуль хостинга обработчика запросов ASP.NET SignalR.
    /// </summary>
    public class SignalROwinHostingModule : IOwinHostingModule
    {
        public SignalROwinHostingModule(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }


        private readonly IDependencyResolver _dependencyResolver;


        public OwinHostingModuleType ModuleType => OwinHostingModuleType.AspNetSignalR;


        public void Configure(IAppBuilder builder, IOwinHostingContext context, ILog log)
        {
            var config = new HubConfiguration
            {
                EnableDetailedErrors = true,
                Resolver = _dependencyResolver
            };

            builder.MapSignalR(config);
        }
    }
}