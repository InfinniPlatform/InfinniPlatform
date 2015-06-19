using InfinniPlatform.Api.Hosting;

namespace InfinniPlatform.Hosting
{
    /// <summary>
    ///     Провайдер локального (без сервисов) роутинга системы
    /// </summary>
    public class LocalDataProvider : IConfigRequestProvider
    {
        private readonly string _action;
        private readonly string _configName;
        private readonly string _metadata;
        private readonly string _userName;
        private readonly string _version;

        public LocalDataProvider(string configName, string metadata, string action, string userName, string version)
        {
            _configName = configName;
            _metadata = metadata;
            _action = action;
            _userName = userName;
            _version = version != "0" ? version : null;
        }

        /// <summary>
        ///     Получить идентификатор метаданных конфигурации из роутинга
        /// </summary>
        /// <returns></returns>
        public string GetConfiguration()
        {
            return _configName;
        }

        /// <summary>
        ///     Получить идентификатор метаданных объекта метаданных из роутинга
        /// </summary>
        /// <returns></returns>
        public string GetMetadataIdentifier()
        {
            return _metadata;
        }

        /// <summary>
        ///     Получить идентификатор метаданных действия из роутинга
        /// </summary>
        /// <returns></returns>
        public string GetServiceName()
        {
            return _action;
        }

        /// <summary>
        ///     Получить идентификатор авторизованного в системе пользователя
        /// </summary>
        /// <returns></returns>
        public string GetUserName()
        {
            return _userName;
        }

        /// <summary>
        ///     Получить идентификатор работающей версии сервисов
        /// </summary>
        /// <returns></returns>
        public string GetVersion()
        {
            return _version;
        }
    }
}