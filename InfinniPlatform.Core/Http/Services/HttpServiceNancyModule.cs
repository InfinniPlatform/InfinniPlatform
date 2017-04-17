using InfinniPlatform.Core.Abstractions.Http;

using Nancy;

namespace InfinniPlatform.Core.Http.Services
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