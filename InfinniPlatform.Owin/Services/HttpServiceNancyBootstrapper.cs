using InfinniPlatform.Sdk.Services;

using Nancy;
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

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register(_nancyModuleCatalog);
        }
    }
}