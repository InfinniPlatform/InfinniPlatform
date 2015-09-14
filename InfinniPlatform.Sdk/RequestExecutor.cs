﻿using System.Collections;
using System.IO;
using System.Net;
using InfinniPlatform.Sdk.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Extensions;

namespace InfinniPlatform.Sdk
{
    public sealed class RequestExecutor 
    {
        private readonly CookieContainer _cookieContainer;

        public RequestExecutor(CookieContainer cookieContainer = null)
        {
            _cookieContainer = cookieContainer;
        }


        public RestQueryResponse QueryGet(string url, string arguments = null)
        {
            var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;

            restClient.Timeout = 1000 * 60 * 200;

            IRestRequest request = null;
            if (arguments != null)
            {
                request = new RestRequest("?{argument}") {RequestFormat = DataFormat.Json}
                    .AddUrlSegment("argument", arguments);
            }
            else
            {
                request = new RestRequest() { RequestFormat = DataFormat.Json };
            }

            IRestResponse restResponse = restClient.Get(request);

            return restResponse.ToQueryResponse();
        }

        public RestQueryResponse QueryPost(string url, object body = null)
        {
            var restClient = new RestClient(url);


            restClient.CookieContainer = _cookieContainer;

            // Изменение времени ожидания ответа сделано для того, чтобы можно было загрузить большие объемы данных
            restClient.Timeout = 1000 * 60 * 300;

            var restRequest = new RestRequest
            {
                RequestFormat = DataFormat.Json
            };

            AddBody(body, restRequest);

            IRestResponse restResponse = restClient.Post(restRequest);

            return restResponse.ToQueryResponse();
        }

        /// <summary>
        ///   Формирование запроса на вставку нового документа
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="body">Тело запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public RestQueryResponse QueryPut(string url, object body = null)
        {
            var restClient = new RestClient(url);


            restClient.CookieContainer = _cookieContainer;
            restClient.Timeout = 1000 * 60 * 300;

            var restRequest = new RestRequest
            {
                RequestFormat = DataFormat.Json
            };

            AddBody(body, restRequest);

            IRestResponse restResponse = restClient.Put(restRequest);

            return restResponse.ToQueryResponse();
        }

        private static void AddBody(object body, RestRequest restRequest)
        {
            if (body != null)
            {
                dynamic bodyObject = body;
                if (!(body is JToken) && !(body is DynamicWrapper))
                {
                    if (body is IEnumerable)
                    {
                        bodyObject = JArray.FromObject(body);
                    }
                    else
                    {
                        bodyObject = JObject.FromObject(body);
                    }
                }
                restRequest.AddParameter("application/json", bodyObject, ParameterType.RequestBody);
            }
        }

        public RestQueryResponse QueryDelete(string url, dynamic body = null)
        {
            var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;

            var restRequest = new RestRequest
            {
                RequestFormat = DataFormat.Json
            };

            AddBody(body, restRequest);

            IRestResponse restResponse = restClient.Delete(restRequest);

            return restResponse.ToQueryResponse();
        }

        public RestQueryResponse QueryGetById(string url)
        {
            var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;
            IRestResponse restResponse = restClient.Get(new RestRequest
            {
                RequestFormat = DataFormat.Json
            });

            return restResponse.ToQueryResponse();
        }

        public RestQueryResponse QueryPostFile(string url,string applicationId, string documentType,  string instanceId, string fieldName, string fileName, Stream fileStream, string sessionId = null)
        {
            var restClient = new RestClient(url);

            restClient.CookieContainer = _cookieContainer;
            restClient.Timeout = 1000 * 60 * 200;

            var linkedData = new
            {
                InstanceId = instanceId,
                FieldName = fieldName,
                FileName = fileName,
                SessionId = sessionId,
                ApplicationId = applicationId,
                DocumentType = documentType
            };

            IRestResponse restResponse = restClient.Post(new RestRequest("?linkedData={argument}") { RequestFormat = DataFormat.Json }
                                                             .AddUrlSegment("argument", JsonConvert.SerializeObject(linkedData))
                                                             .AddFile(fileName, fileStream.ReadAsBytes(), fileName, "multipart/form-data"));
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
