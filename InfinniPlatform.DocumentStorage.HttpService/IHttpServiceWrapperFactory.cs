using InfinniPlatform.Core.Http;

namespace InfinniPlatform.DocumentStorage.HttpService
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