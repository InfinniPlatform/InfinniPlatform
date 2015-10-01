using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Compression;
using InfinniPlatform.Hosting;
using InfinniPlatform.WebApi.ConfigRequestProviders;
using InfinniPlatform.WebApi.WebApi;
using Newtonsoft.Json;

namespace InfinniPlatform.WebApi.Controllers
{
    public sealed class UploadController : ApiController
    {
        private readonly IApiControllerFactory _apiControllerFactory;
        private readonly IHttpResultHandlerFactory _resultHandlerFactory;

        public UploadController(IApiControllerFactory apiControllerFactory, IHttpResultHandlerFactory resultHandlerFactory)
        {
            _apiControllerFactory = apiControllerFactory;
            _resultHandlerFactory = resultHandlerFactory;
        }

        private string GetUserName()
        {
            return (User != null && !string.IsNullOrEmpty(User.Identity.Name)) ? User.Identity.Name : AuthorizationStorageExtensions.UnknownUser;
        }

        private IRestVerbsContainer GetMetadata()
        {
            var metadata = Request.GetRouteData().Values.ContainsKey("metadata") ? 
                _apiControllerFactory.GetTemplate((string)Request.GetRouteData().Values["configuration"],
                (string)Request.GetRouteData().Values["metadata"], GetUserName()) : null;
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
        public HttpResponseMessage ProcessPostVerb([FromUri] dynamic linkedData)
        {
            var dataProvider = Request.Content.ReadAsMultipartAsync().Result;

	        var argument = JsonConvert.DeserializeObject(linkedData);

            var verbProcessor = GetMetadata().FindUploadVerb(GetServiceName(), argument, dataProvider.Contents.First().ReadAsStreamAsync().Result);

            var httpResultHandler = _resultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

            var result = httpResultHandler.WrapResult(InvokeRestVerb(verbProcessor));
            return result;
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
					UserName = GetUserName()
                });
            }
        }

    }
}
