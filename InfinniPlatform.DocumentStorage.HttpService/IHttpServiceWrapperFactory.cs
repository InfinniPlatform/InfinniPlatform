using InfinniPlatform.Http;

using Microsoft.AspNetCore.Mvc;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Предоставляет интерфейс для создания типизированных декораторов для экземпляров <see cref="IHttpService"/>.
    /// </summary>
    public interface IHttpServiceWrapperFactory
    {
        /// <summary>
        /// Создать типизированный декоратор для экземпляра <see cref="IHttpService"/>.
        /// </summary>
        IHttpService CreateServiceWrapper(string httpServiceWrapperTypeName, IHttpService httpService);
    }
}