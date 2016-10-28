using System.Collections.Generic;
using InfinniPlatform.Sdk.Documents.Services;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.DocumentStorage.Services
{
    /// <summary>
    /// Предоставляет интерфейс для создания сервисов по работе с документами.
    /// </summary>
    internal interface IDocumentHttpServiceFactory
    {
        /// <summary>
        /// Создает сервисы по работе с документами на основе указанного обработчика.
        /// </summary>
        /// <param name="httpServiceHandler">Обработчик для сервиса по работе с документами.</param>
        IEnumerable<IHttpService> CreateServices(IDocumentHttpServiceHandlerBase httpServiceHandler);
    }
}