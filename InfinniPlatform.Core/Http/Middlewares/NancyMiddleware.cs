using Microsoft.AspNetCore.Builder;

using Nancy.Bootstrapper;
using Nancy.Owin;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Модуль хостинга для обработки прикладных запросов на базе Nancy.
    /// </summary>
    internal class NancyMiddleware : IApplicationMiddleware
    {
        public NancyMiddleware(INancyBootstrapper nancyBootstrapper)
        {
            _nancyBootstrapper = nancyBootstrapper;
        }


        private readonly INancyBootstrapper _nancyBootstrapper;


        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(x => x.UseNancy(new NancyOptions
                                        {
                                            Bootstrapper = _nancyBootstrapper
                                        }));
        }
    }
}