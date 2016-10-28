using InfinniPlatform.Http.Middlewares;

using Microsoft.AspNet.SignalR;

using Owin;

namespace InfinniPlatform.PushNotification.Owin
{
    /// <summary>
    /// Модуль хостинга обработчика запросов ASP.NET SignalR.
    /// </summary>
    internal class SignalRHttpMiddleware : HttpMiddleware
    {
        public SignalRHttpMiddleware(IDependencyResolver dependencyResolver) : base(HttpMiddlewareType.AfterAuthentication)
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