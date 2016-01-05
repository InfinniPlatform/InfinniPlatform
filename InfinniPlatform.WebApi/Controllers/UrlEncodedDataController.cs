using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.RestApi.Auth;
using InfinniPlatform.Core.RestQuery;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.WebApi.ConfigRequestProviders;

namespace InfinniPlatform.WebApi.Controllers
{
    public sealed class UrlEncodedDataController : ApiController
    {
        public UrlEncodedDataController(IApiControllerFactory apiControllerFactory, IHttpResultHandlerFactory resultHandlerFactory)
        {
            _apiControllerFactory = apiControllerFactory;
            _resultHandlerFactory = resultHandlerFactory;
        }


        private readonly IApiControllerFactory _apiControllerFactory;
        private readonly IHttpResultHandlerFactory _resultHandlerFactory;


        private IRestVerbsContainer GetMetadata()
        {
            IRestVerbsContainer restVerbsContainer = null;

            object configuration = null;
            object documentType = null;
            var routeData = Request.GetRouteData();

            if (routeData.Values.TryGetValue("configuration", out configuration) && routeData.Values.TryGetValue("metadata", out documentType))
            {
                restVerbsContainer = _apiControllerFactory.GetTemplate((string)configuration, (string)documentType);
            }

            if (restVerbsContainer == null)
            {
                throw new ArgumentException($"Не найдены метаданные для {documentType}. Используйте метод InstallServices для регистрации обработчиков.");
            }

            return restVerbsContainer;
        }

        private string GetServiceName()
        {
            return Request.GetRouteData().Values.ContainsKey("service") ? (string)Request.GetRouteData().Values["service"] : string.Empty;
        }

        [HttpPost]
        public HttpResponseMessage ProcessPostVerb()
        {
            var verbProcessor = GetMetadata().FindVerbUrlEncodedData(GetServiceName(), GetParamsDictionary());

            var httpResultHandler = _resultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

            var result = httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor));
            return result;
        }

        [HttpGet]
        public HttpResponseMessage ProcessGetVerb()
        {
            var verbProcessor = GetMetadata().FindVerbUrlEncodedData(GetServiceName(), GetParamsFromUrl());

            var httpResultHandler = _resultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

            var result = httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor));
            return result;
        }

        private dynamic GetParamsFromUrl()
        {
            var dataProvider = GetQueryString(Request, "Form");

            return dataProvider.ToDynamic();
        }

        /// <summary>
        /// Returns an individual querystring value
        /// </summary>
        /// <param name="request"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();
            if (queryStrings == null)
            {
                return null;
            }

            var match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, key, true) == 0);
            if (string.IsNullOrEmpty(match.Value))
            {
                return null;
            }

            return match.Value;
        }

        private dynamic GetParamsDictionary()
        {
            var dataProvider = Request.Content.ReadAsStringAsync().Result;

            return dataProvider.ToDynamic();
        }

        private object InvokeRestVerb(TargetDelegate verbProcessor)
        {
            if (verbProcessor != null)
            {
                SetContext(verbProcessor);
                return verbProcessor.Invoke();
            }
            return null;
        }

        private string GetUserName()
        {
            return (User != null && !string.IsNullOrEmpty(User.Identity.Name))
                ? User.Identity.Name
                : AuthorizationStorageExtensions.UnknownUser;
        }

        private void SetContext(TargetDelegate invokationInfo)
        {
            var prop = invokationInfo.Target.GetType().GetProperties().FirstOrDefault(p => p.PropertyType.IsAssignableFrom(typeof(IConfigRequestProvider)));
            if (prop != null)
            {
                prop.SetValue(invokationInfo.Target, new ConfigRequestProvider
                {
                    RequestData = Request.GetRouteData(),
                    UserName = GetUserName()
                });
            }
        }
    }
}