using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Logging;

using Nancy.Bootstrapper;
using Nancy.Owin;

using Owin;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Модуль хостинга для обработки прикладных запросов на базе Nancy.
    /// </summary>
    internal sealed class NancyOwinHostingModule : IOwinHostingModule
    {
        public NancyOwinHostingModule(INancyBootstrapper nancyBootstrapper)
        {
            _nancyBootstrapper = nancyBootstrapper;
        }

        private readonly INancyBootstrapper _nancyBootstrapper;

        public OwinHostingModuleType ModuleType => OwinHostingModuleType.Application;

        public void Configure(IAppBuilder builder, IOwinHostingContext context, ILog log)
        {
            builder.UseNancy(new NancyOptions
            {
                Bootstrapper = _nancyBootstrapper
            });
        }
    }
}