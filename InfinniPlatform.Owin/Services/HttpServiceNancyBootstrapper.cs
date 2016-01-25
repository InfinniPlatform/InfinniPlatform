using InfinniPlatform.Sdk.Services;

using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace InfinniPlatform.Owin.Services
{
    /// <summary>
    /// Настройки Nancy для <see cref="IHttpService" />
    /// </summary>
    internal sealed class HttpServiceNancyBootstrapper : DefaultNancyBootstrapper
    {
        public HttpServiceNancyBootstrapper(INancyModuleCatalog nancyModuleCatalog)
        {
            _nancyModuleCatalog = nancyModuleCatalog;
        }

        private readonly INancyModuleCatalog _nancyModuleCatalog;

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                var internalConfiguration = base.InternalConfiguration;
                internalConfiguration.CultureService = typeof(HttpServiceNancyCultureService);
                return internalConfiguration;
            }
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register(_nancyModuleCatalog);
        }
    }
}