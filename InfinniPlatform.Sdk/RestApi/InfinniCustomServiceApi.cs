using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// API для вызова кастомного сервиса
    /// </summary>
    public sealed class InfinniCustomServiceApi : BaseApi, ICustomServiceApi
    {
        public InfinniCustomServiceApi(string server, int port) : base(server, port)
        {
        }

        /// <summary>
        /// Выполнить вызов пользовательского сервиса
        /// </summary>
        /// <returns>Клиентская сессия</returns>
        public dynamic ExecuteAction(string application, string documentType, string service, dynamic body)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrl(application, documentType, service), body);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInvokeCustomService, response));
        }
    }
}