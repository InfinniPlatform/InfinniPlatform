using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.IoC;

using Nancy;

namespace InfinniPlatform.Http.Services
{
    /// <summary>
    /// Каталог модулей Nancy для сервисов <see cref="IHttpService" />.
    /// </summary>
    internal class HttpServiceNancyModuleCatalog : INancyModuleCatalog
    {
        public HttpServiceNancyModuleCatalog(IContainerResolver containerResolver, HttpServiceNancyModuleInitializer httpServiceNancyModuleInitializer)
        {
            _containerResolver = containerResolver;
            _httpServiceNancyModuleInitializer = httpServiceNancyModuleInitializer;
        }


        private readonly IContainerResolver _containerResolver;
        private readonly HttpServiceNancyModuleInitializer _httpServiceNancyModuleInitializer;


        public IEnumerable<INancyModule> GetAllModules(NancyContext context)
        {
            var moduleTypes = _httpServiceNancyModuleInitializer.GetModuleTypes();
            return moduleTypes.Select(t => (INancyModule)_containerResolver.Resolve(t));
        }

        public INancyModule GetModule(Type moduleType, NancyContext context)
        {
            return (INancyModule)_containerResolver.Resolve(moduleType);
        }
    }
}