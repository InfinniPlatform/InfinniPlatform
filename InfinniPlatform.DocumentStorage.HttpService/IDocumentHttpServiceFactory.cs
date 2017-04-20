using System.Collections.Generic;

using InfinniPlatform.Core.Http;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Предоставляет интерфейс для создания сервисов по работе с документами.
    /// </summary>
    public interface IDocumentHttpServiceFactory
    {
        /// <summary>
        /// Создает сервисы по работе с документами на основе указанного обработчика.
        /// </summary>
        /// <param name="httpServiceHandler">Обработчик для сервиса по работе с документами.</param>
        IEnumerable<IHttpService> CreateServices(IDocumentHttpServiceHandlerBase httpServiceHandler);
    }
}