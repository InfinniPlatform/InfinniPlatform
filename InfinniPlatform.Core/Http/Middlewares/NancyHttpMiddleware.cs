using InfinniPlatform.Http.Middlewares;

using Nancy.Bootstrapper;
using Nancy.Owin;

using Owin;

namespace InfinniPlatform.Core.Http.Middlewares
{
    /// <summary>
    /// Модуль хостинга для обработки прикладных запросов на базе Nancy.
    /// </summary>
    internal class NancyHttpMiddleware : HttpMiddleware
    {
        public NancyHttpMiddleware(INancyBootstrapper nancyBootstrapper) : base(HttpMiddlewareType.Application)
        {
            _nancyBootstrapper = nancyBootstrapper;
        }


        private readonly INancyBootstrapper _nancyBootstrapper;


        public override void Configure(IAppBuilder builder)
        {
            builder.UseNancy(new NancyOptions { Bootstrapper = _nancyBootstrapper });
        }
    }
}