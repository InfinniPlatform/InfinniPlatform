using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Hosting;

using Microsoft.AspNet.SignalR;

using Owin;

namespace InfinniPlatform.PushNotification.Owin
{
    /// <summary>
    /// Модуль хостинга обработчика запросов ASP.NET SignalR.
    /// </summary>
    internal class SignalROwinHostingMiddleware : OwinHostingMiddleware
    {
        public SignalROwinHostingMiddleware(IDependencyResolver dependencyResolver) : base(HostingMiddlewareType.AfterAuthentication)
        {
            _dependencyResolver = dependencyResolver;
        }


        private readonly IDependencyResolver _dependencyResolver;


        public override void Configure(IAppBuilder builder)
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