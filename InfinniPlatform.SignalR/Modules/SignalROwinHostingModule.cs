using InfinniPlatform.Owin.Modules;

using Microsoft.AspNet.SignalR;

using Owin;

namespace InfinniPlatform.SignalR.Modules
{
    /// <summary>
    /// Модуль хостинга обработчика запросов ASP.NET SignalR.
    /// </summary>
    internal sealed class SignalROwinHostingModule : IOwinHostingModule
    {
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.AspNetSignalR;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            var config = new HubConfiguration
            {
                EnableDetailedErrors = true
            };

            builder.MapSignalR(config);
        }
    }
}