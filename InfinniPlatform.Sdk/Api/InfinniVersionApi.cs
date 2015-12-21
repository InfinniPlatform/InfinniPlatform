using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.Api
{
    /// <summary>
    /// API для управления версиями конфигураций
    /// </summary>
    public sealed class InfinniVersionApi : BaseApi
    {
        public InfinniVersionApi(string server, int port) : base(server, port)
        {
        }

        /// <summary>
        /// Получить список неактуальных версий для зарегистрированного пользователя
        /// </summary>
        /// <returns>Список неактуальных версий</returns>
        public dynamic GetIrrelevantVersions(string userName)
        {
            var response = RequestExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlVersion(userName));

            return ProcessAsObjectResult(response, string.Format(Resources.FailToGetIrrelevantVersions, response));
        }

        /// <summary>
        /// Установить актуальные версии для пользователя
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="version">Настройка актуальной версии </param>
        /// <returns>Результат установки</returns>
        public dynamic SetRelevantVersion(string userName, dynamic version)
        {
            version = DynamicWrapperExtensions.ToDynamic(version);

            version.UserName = userName;

            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlVersion(userName), version);

            return ProcessAsObjectResult(response, string.Format(Resources.FailToSetUserRelevantVersions, response.GetErrorContent()));
        }
    }
}