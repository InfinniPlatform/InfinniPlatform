using Microsoft.AspNetCore.Builder;

using Nancy.Bootstrapper;
using Nancy.Owin;

namespace InfinniPlatform.Http.Middlewares
{
    /// <summary>
    /// Hosting layer for request processing based on NancyFx framework.
    /// </summary>
    internal class NancyAppLayer : IBusinessAppLayer, IDefaultAppLayer
    {
        public NancyAppLayer(INancyBootstrapper nancyBootstrapper)
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