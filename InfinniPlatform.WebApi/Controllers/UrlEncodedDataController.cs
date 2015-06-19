using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.Application.Dynamic;
using InfinniPlatform.WebApi.ConfigRequestProviders;

namespace InfinniPlatform.WebApi.Controllers
{
	public sealed class UrlEncodedDataController : ApiController
	{
		private readonly IApiControllerFactory _apiControllerFactory;
        private readonly IHttpResultHandlerFactory _resultHandlerFactory;

		public UrlEncodedDataController(IApiControllerFactory apiControllerFactory, IHttpResultHandlerFactory resultHandlerFactory)
        {
            _apiControllerFactory = apiControllerFactory;
            _resultHandlerFactory = resultHandlerFactory;
        }

        private IRestVerbsContainer GetMetadata()
        {
            var metadata = Request.GetRouteData().Values.ContainsKey("metadata") ? _apiControllerFactory.GetTemplate((string)Request.GetRouteData().Values["version"], (string)Request.GetRouteData().Values["configuration"],
                (string)Request.GetRouteData().Values["metadata"]) : null;
            if (metadata == null)
            {
                throw new ArgumentException(string.Format("Не найдены метаданные для {0}. Используйте метод InstallServices для регистрации обработчиков.", Request.GetRouteData().Values["metadata"]));
            }
            return metadata;
        }

        private string GetServiceName()
        {
            return Request.GetRouteData().Values.ContainsKey("service") ? (string)Request.GetRouteData().Values["service"] : string.Empty;
        }

        [HttpPost]
        public HttpResponseMessage ProcessPostVerb()
        {            
            var verbProcessor = GetMetadata().FindVerbUrlEncodedData(GetServiceName(), GetParamsDictionary() );

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
				return null;

			var match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, key, true) == 0);
			if (string.IsNullOrEmpty(match.Value))
				return null;

			return match.Value;
		}


		private dynamic GetParamsDictionary()
		{
			var dataProvider = Request.Content.ReadAsFormDataAsync().Result;

			return dataProvider.Get("Form").ToDynamic();			
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

        private void SetContext(TargetDelegate invokationInfo)
        {
            var prop = invokationInfo.Target.GetType().GetProperties().FirstOrDefault(p => p.PropertyType.IsAssignableFrom(typeof(IConfigRequestProvider)));
            if (prop != null)
            {
                prop.SetValue(invokationInfo.Target, new ConfigRequestProvider()
                {
                    RequestData = Request.GetRouteData(),
					UserName = (User != null && !string.IsNullOrEmpty(User.Identity.Name)) ? User.Identity.Name : AuthorizationStorageExtensions.UnknownUser
                });
            }
        }
	}
}
