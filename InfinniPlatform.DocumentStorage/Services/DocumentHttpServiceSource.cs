using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents.Services;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.DocumentStorage.Services
{
    /// <summary>
    /// Источник сервисов по работе с документами.
    /// </summary>
    internal sealed class DocumentHttpServiceSource : IHttpServiceSource
    {
        public DocumentHttpServiceSource(IDocumentHttpServiceFactory httpServiceFactory,
                                         IHttpServiceWrapperFactory httpServiceWrapperFactory,
                                         IEnumerable<IDocumentHttpServiceHandlerBase> httpServiceHandlers)
        {
            _httpServiceFactory = httpServiceFactory;
            _httpServiceWrapperFactory = httpServiceWrapperFactory;
            _httpServiceHandlers = httpServiceHandlers;
        }


        private readonly IDocumentHttpServiceFactory _httpServiceFactory;
        private readonly IHttpServiceWrapperFactory _httpServiceWrapperFactory;
        private readonly IEnumerable<IDocumentHttpServiceHandlerBase> _httpServiceHandlers;


        public IEnumerable<IHttpService> GetServices()
        {
            var index = 0;

            foreach (var handler in _httpServiceHandlers)
            {
                var services = _httpServiceFactory.CreateServices(handler);

                foreach (var service in services)
                {
                    var serviceWrapper = _httpServiceWrapperFactory.CreateServiceWrapper($"DocumentHttpService{index++}", service);

                    yield return serviceWrapper;
                }
            }
        }
    }
}