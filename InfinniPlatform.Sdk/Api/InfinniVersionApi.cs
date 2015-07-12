using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Properties;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Sdk.Api
{
    /// <summary>
    /// API для управления версиями конфигураций
    /// </summary>
    public sealed class InfinniVersionApi : BaseApi
    {
        public InfinniVersionApi(string server, string port) : base(server, port)
        {
        }

        /// <summary>
        /// Получить список неактуальных версий для зарегистрированного пользователя
        /// </summary>
        /// <returns>Список неактуальных версий</returns>
        public dynamic GetIrrelevantVersions(string userName)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            var response = restQueryExecutor.QueryGet(RouteBuilder.BuildRestRoutingUrlVersion(userName));

            return ProcessAsArrayResult(response, string.Format(Resources.FailToGetIrrelevantVersions, response.GetErrorContent())); 
        }


        /// <summary>
        ///   Установить актуальные версии для пользователя
        /// </summary>
        /// <param name="userName">Пользователь</param>
        /// <param name="version">Настройка актуальной версии </param>
        /// <returns>Результат установки</returns>
        public dynamic SetRelevantVersion(string userName, dynamic version)
        {
            var restQueryExecutor = new RequestExecutor(CookieContainer);

            version = DynamicWrapperExtensions.ToDynamic(version);

            version.UserName = userName;
            
            var response = restQueryExecutor.QueryPost(RouteBuilder.BuildRestRoutingUrlVersion(userName), version);

            return ProcessAsObjectResult(response, string.Format(Resources.FailToSetUserRelevantVersions, response.GetErrorContent())); 
        }
    }
}
