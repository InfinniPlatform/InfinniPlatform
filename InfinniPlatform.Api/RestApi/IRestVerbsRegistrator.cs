using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Api.RestApi
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