using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using InfinniPlatform.Api.Dynamic;
using Newtonsoft.Json;
using RestSharp;

namespace InfinniPlatform.Api.RestQuery.RestQueryExecutors
{
	public sealed class RestQueryExecutor : IRestQueryExecutor
	{
		private readonly CookieContainer _cookieContainer;

		public RestQueryExecutor(CookieContainer cookieContainer = null)
		{
			_cookieContainer = cookieContainer;
		}


		public RestQueryResponse QueryGet(string url, object queryObject)
		{
			var restClient = new RestClient(url);

			restClient.CookieContainer = _cookieContainer;

			restClient.Timeout = 1000 * 60 * 200;
			var argumentsString = JsonConvert.SerializeObject(queryObject);
			IRestResponse restResponse = restClient.Get(new RestRequest("?query={argument}") { RequestFormat = DataFormat.Json }
															.AddUrlSegment("argument", argumentsString));

			return restResponse.ToQueryResponse();
		}

		public RestQueryResponse QueryPost(string url, object body)
		{
            // TODO: Необходимо переделать реализацию и использовать шину сообщений, пока сделано простое увеличение таймаута до 300 минут 
		    var restClient = new RestClient(url)
		    {
		        CookieContainer = _cookieContainer, 
                Timeout = 1000*60*300
		    };

		    var restRequest = new RestRequest
            {
                RequestFormat = DataFormat.Json
            };

            restRequest.AddBody(body);
            
            IRestResponse restResponse = restClient.Post(restRequest);

			return restResponse.ToQueryResponse();
		}


		public RestQueryResponse QueryPostFile(string url, object linkedData, string filePath)
		{
			var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;
			restClient.Timeout = 1000 * 60 * 200;

			IRestResponse restResponse = restClient.Post(new RestRequest("?linkedData={argument}") { RequestFormat = DataFormat.Json }
															 .AddUrlSegment("argument", JsonConvert.SerializeObject(linkedData))
															 .AddFile(Path.GetFileName(filePath), File.ReadAllBytes(filePath), Path.GetFileName(filePath), "multipart/form-data"));
			return restResponse.ToQueryResponse();
		}

		public RestQueryResponse QueryPostUrlEncodedData(string url, object formData)
		{
			var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;
			restClient.Timeout = 1000 * 60 * 200;

			var request = new RestRequest("", Method.POST);
			request.AddParameter("Form", JsonConvert.SerializeObject(formData));
			var response = restClient.Execute(request);
			return response.ToQueryResponse();

		}


		public RestQueryResponse QueryGetUrlEncodedData(string url, object formData)
		{
			var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;
			restClient.Timeout = 1000 * 60 * 200;

			var argumentsString = JsonConvert.SerializeObject(formData);
			IRestResponse restResponse = restClient.Get(new RestRequest("?Form={formData}") { RequestFormat = DataFormat.Json }
															.AddUrlSegment("formData", argumentsString));
			return restResponse.ToQueryResponse();
		}

	}
}
