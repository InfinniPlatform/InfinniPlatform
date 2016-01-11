using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Core.Hosting
{
    public interface IRestVerbsRegistrator
    {
        /// <summary>
        /// Добавить обработчик методов сервиса REST
        /// </summary>
        /// <returns>Контейнер обработчиков REST запросов</returns>
        IRestVerbsContainer AddVerb(IQueryHandler queryHandler);
    }
}