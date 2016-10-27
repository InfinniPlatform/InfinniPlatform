using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Hosting;

using Nancy.Bootstrapper;
using Nancy.Owin;

using Owin;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Модуль хостинга для обработки прикладных запросов на базе Nancy.
    /// </summary>
    internal class NancyOwinHostingMiddleware : OwinHostingMiddleware
    {
        public NancyOwinHostingMiddleware(INancyBootstrapper nancyBootstrapper) : base(HostingMiddlewareType.Application)
        {
            _nancyBootstrapper = nancyBootstrapper;
        }


        private readonly INancyBootstrapper _nancyBootstrapper;


        public override void Configure(IAppBuilder builder)
        {
            builder.UseNancy(new NancyOptions
            {
                Bootstrapper = _nancyBootstrapper
            });
        }
    }
}