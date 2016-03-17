using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.DocumentStorage.Services
{
    /// <summary>
    /// Предоставляет интерфейс для создания типизированных декораторов для экземпляров <see cref="IHttpService"/>.
    /// </summary>
    internal interface IHttpServiceWrapperFactory
    {
        /// <summary>
        /// Создать типизированный декоратор для экземпляра <see cref="IHttpService"/>.
        /// </summary>
        IHttpService CreateServiceWrapper(string httpServiceWrapperTypeName, IHttpService httpService);
    }
}