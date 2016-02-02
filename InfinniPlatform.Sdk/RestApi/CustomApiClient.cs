using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// Реализует REST-клиент для CustomApi.
    /// </summary>
    public sealed class CustomApiClient : BaseRestClient
    {
        public CustomApiClient(string server, int port, IJsonObjectSerializer serializer = null) : base(server, port, serializer)
        {
        }

        /// <summary>
        /// Выполнить вызов пользовательского сервиса
        /// </summary>
        /// <returns>Клиентская сессия</returns>
        public object ExecuteAction(string configuration, string documentType, string actionName, object body)
        {
            var requestUri = BuildRequestUri($"/{configuration}/StandardApi/{documentType}/{actionName}");

            return RequestExecutor.PostObject(requestUri, body);
        }
    }
}