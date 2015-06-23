using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.Properties;
using InfinniPlatform.Sdk.Properties;

namespace InfinniPlatform.Sdk.Api
{

    /// <summary>
    ///  API для вызова кастомного сервиса
    /// </summary>
    public sealed class InfinniCustomServiceApi : BaseApi
    {
        public InfinniCustomServiceApi(string server, string port, string version)
            : base(server, port, version)
        {
        }

        /// <summary>
        ///   Выполнить вызов пользовательского сервиса
        /// </summary>
        /// <returns>Клиентская сессия</returns>
        public dynamic ExecuteAction(string application, string documentType, string service, dynamic body)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrl(Version, application, documentType,service), body);

            return ProcessAsObjectResult(response, string.Format(Resources.UnableToInvokeCustomService, response.GetErrorContent()));
        }
    }
}
