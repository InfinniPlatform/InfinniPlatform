using Nancy;

namespace InfinniPlatform.Http
{
    /// <summary>
    /// Модуль Nancy для сервиса <see cref="IHttpService"/>.
    /// </summary>
    internal class HttpServiceNancyModule<TService> : NancyModule where TService : IHttpService
    {
        public HttpServiceNancyModule(HttpServiceNancyModuleInitializer initializer)
        {
            ModulePath = initializer.GetModulePath<TService>();

            initializer.InitializeModuleRoutes<TService>(this);
        }
    }
}