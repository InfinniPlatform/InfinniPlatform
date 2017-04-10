using InfinniPlatform.Http.Middlewares;
using Microsoft.AspNetCore.Builder;
using Nancy.Bootstrapper;
using Nancy.Owin;

namespace InfinniPlatform.Core.Http.Middlewares
{
    /// <summary>
    /// Модуль хостинга для обработки прикладных запросов на базе Nancy.
    /// </summary>
    internal class NancyHttpMiddleware : HttpMiddlewareBase<NancyMiddlewareOptions>
    {
        private readonly INancyBootstrapper _nancyBootstrapper;

        public NancyHttpMiddleware(INancyBootstrapper nancyBootstrapper) : base(HttpMiddlewareType.Application)
        {
            _nancyBootstrapper = nancyBootstrapper;
        }


        public override void Configure(IApplicationBuilder app, NancyMiddlewareOptions options)
        {
            app.UseOwin(x => x.UseNancy(new NancyOptions
                                        {
                                            Bootstrapper = _nancyBootstrapper
                                        }));
        }
    }
}