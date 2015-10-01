using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Compression;
using InfinniPlatform.Json;
using InfinniPlatform.WebApi.ConfigRequestProviders;

namespace InfinniPlatform.WebApi.Controllers
{
	public sealed class StandardStreamedApiController : ApiController
	{

		private readonly IApiControllerFactory _apiControllerFactory;
		private readonly IDataCompressor _dataCompressor;
		private readonly IIndexFactory _indexFactory;
	    private readonly IHttpResultHandlerFactory _resultHandlerFactory;

	    public StandardStreamedApiController(IApiControllerFactory apiControllerFactory, IDataCompressor dataCompressor, IIndexFactory indexFactory, IHttpResultHandlerFactory resultHandlerFactory)
        {
            _apiControllerFactory = apiControllerFactory;
            _dataCompressor = dataCompressor;
            _indexFactory = indexFactory;
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
			var jsonDecompressedDataStream = new MemoryStream();
		
			using (var jsoncompressedDataStream = Request.Content.ReadAsStreamAsync().Result)
			{
				_dataCompressor.Decompress(jsoncompressedDataStream, jsonDecompressedDataStream);
			}

			var summaryData = new List<object>();
			using (jsonDecompressedDataStream)
			{
				var jsonDataStreamEnumerable = new JsonArrayStreamEnumerable(jsonDecompressedDataStream);

				foreach (dynamic data in jsonDataStreamEnumerable)
				{
					var verbArguments = data.ToObject<ExpandoObject>();
					var verbProcessor = GetMetadata().FindVerbPost(GetServiceName(), verbArguments);
					var resultData = InvokeRestVerb(verbProcessor);

				    var handler = _resultHandlerFactory.GetResultHandler(verbProcessor.HttpResultHandler);

					var response = handler.WrapResult(resultData);

					if (response.StatusCode == HttpStatusCode.BadRequest || response.StatusCode == HttpStatusCode.NotFound ||
						response.StatusCode == HttpStatusCode.InternalServerError)
					{
						return response;
					}
					summaryData.Add(resultData);
				}

			}
			_indexFactory.BuildIndexStateProvider().Refresh();

			return Request.CreateResponse(HttpStatusCode.OK, summaryData, Configuration.Formatters.JsonFormatter);			
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
				prop.SetValue(invokationInfo.Target, new ConfigRequestProvider
				{
					RequestData = Request.GetRouteData()
				});
			}
		}
	}
}