using Microsoft.AspNetCore.Builder;

using Nancy.Bootstrapper;
using Nancy.Owin;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Модуль хостинга для обработки прикладных запросов на базе Nancy.
    /// </summary>
    internal class NancyHttpMiddleware : HttpMiddlewareBase<NancyMiddlewareOptions>
    {
        public NancyHttpMiddleware(INancyBootstrapper nancyBootstrapper) : base(HttpMiddlewareType.Application)
        {
            _nancyBootstrapper = nancyBootstrapper;
        }


        private readonly INancyBootstrapper _nancyBootstrapper;


        public override void Configure(IApplicationBuilder app, NancyMiddlewareOptions options)
        {
            app.UseOwin(x => x.UseNancy(new NancyOptions
                                        {
                                            Bootstrapper = _nancyBootstrapper
                                        }));
        }
    }
}