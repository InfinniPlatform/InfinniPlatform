using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.Api
{
    /// <summary>
    /// API для вызова кастомного сервиса
    /// </summary>
    public sealed class InfinniCustomServiceApi : BaseApi, ICustomServiceApi
    {
        public InfinniCustomServiceApi(string server, string port, string route)
            : base(server, port, route)
        {
        }

        /// <summary>
        /// Выполнить вызов пользовательского сервиса
        /// </summary>
        /// <returns>Клиентская сессия</returns>
        public dynamic ExecuteAction(string application, string documentType, string service, dynamic body)
        {
            var response = RequestExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrl(application, documentType, service), body);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInvokeCustomService, response.GetErrorContent()));
        }
    }
}