using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using InfinniPlatform.Hosting.WebApi.Implementation.ConfigRequestProviders;
using InfinniPlatform.Hosting.WebApi.Implementation.WebApi;
using Newtonsoft.Json;

namespace InfinniPlatform.Hosting.WebApi.Implementation.Controllers
{
	/// <summary>
	///   REST controller that invokes specified verbs and returns responses
	/// </summary>
    public sealed class StandardApiController : ApiController
    {
        private readonly IApiControllerFactory _apiControllerFactory;

        public StandardApiController(IApiControllerFactory apiControllerFactory)
        {
            _apiControllerFactory = apiControllerFactory;
        }

        private IRestVerbsContainer GetMetadata()
        {
            var metadata =  Request.GetRouteData().Values.ContainsKey("metadata") ? _apiControllerFactory.GetTemplate((string)Request.GetRouteData().Values["metadata"]) : 
				_apiControllerFactory.GetTemplate("configuration");
			if (metadata == null)
			{
				throw new ArgumentException(string.Format("Не найдены метаданные для {0}", Request.GetRouteData().Values["metadata"]));
			}
	        return metadata;
        }

        private string GetServiceName()
        {
            return  Request.GetRouteData().Values.ContainsKey("service") ? (string)Request.GetRouteData().Values["service"] : string.Empty;
        }

		[HttpPost]
        public HttpResponseMessage ProcessPostVerb()
        {
            var arguments = JsonConvert.DeserializeObject<IDictionary<string,object>>(Request.Content.ReadAsStringAsync().Result);
			var verbProcessor = GetMetadata().FindVerbPost(GetServiceName(), arguments );
		    return verbProcessor.HttpResultHandler.WrapResult(InvokeRestVerb(verbProcessor));
        }


        [HttpGet]
        public HttpResponseMessage ProcessGetVerb(string query)
        {
			var arguments = string.IsNullOrEmpty(query) ? new Dictionary<string, object>() : JsonConvert.DeserializeObject<Dictionary<string, object>>(query);
            var verbProcessor = GetMetadata().FindVerbGet(GetServiceName(), arguments);
            return verbProcessor.HttpResultHandler.WrapResult(InvokeRestVerb(verbProcessor));

        }

		[HttpPut]
		public HttpResponseMessage ProcessPutVerb()
		{
			var arguments = JsonConvert.DeserializeObject<IDictionary<string, object>>(Request.Content.ReadAsStringAsync().Result);
			var verbProcessor = GetMetadata().FindVerbPut(GetServiceName(), arguments );
            return verbProcessor.HttpResultHandler.WrapResult(InvokeRestVerb(verbProcessor));

		}

		[HttpDelete]
		public HttpResponseMessage ProcessDeleteVerb([FromUri] IEnumerable<string> items)
		{
			throw new ArgumentException("Currently not implemented");
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
                    RequestData = Request.GetRouteData()
                });
            }
        }
    }
}