using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;

using Newtonsoft.Json;

namespace InfinniPlatform.Sdk.RestApi
{
    public sealed class RequestExecutor
    {
        [ThreadStatic]
        private static RequestExecutor _instance;

        public RequestExecutor()
        {
            var clientHandler = new HttpClientHandler { CookieContainer = new CookieContainer() };
            var client = new HttpClient(clientHandler);
#if DEBUG
            client.Timeout = Timeout.InfiniteTimeSpan;
#endif
            _client = client;
        }


        private readonly HttpClient _client;


        public static RequestExecutor Instance => _instance ?? (_instance = new RequestExecutor());


        public RestQueryResponse QueryGet(string url, string arguments = null)
        {
            var urlBuilder = new StringBuilder(url.Trim('/'));

            if (!string.IsNullOrEmpty(arguments))
            {
                urlBuilder.Append($"/?{arguments}");
            }

            var response = _client.GetAsync(urlBuilder.ToString()).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new RestQueryResponse
                   {
                       Content = content,
                       HttpStatusCode = response.StatusCode
                   };
        }

        public RestQueryResponse QueryPost(string url, object body = null)
        {
            var jsonRequestContent = CreateJsonHttpContent(body);

            var response = _client.PostAsync(url, jsonRequestContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new RestQueryResponse
                   {
                       Content = content,
                       HttpStatusCode = response.StatusCode
                   };
        }

        /// <summary>
        /// Формирование запроса на вставку нового документа
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="body">Тело запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public RestQueryResponse QueryPut(string url, object body = null)
        {
            var jsonRequestContent = CreateJsonHttpContent(body);

            var response = _client.PutAsync(url, jsonRequestContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new RestQueryResponse
                   {
                       Content = content,
                       HttpStatusCode = response.StatusCode
                   };
        }

        public RestQueryResponse QueryDelete(string url, dynamic body = null)
        {
            var response = _client.DeleteAsync(url).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new RestQueryResponse
                   {
                       Content = content,
                       HttpStatusCode = response.StatusCode
                   };
        }

        public RestQueryResponse QueryGetById(string url)
        {
            var response = _client.GetAsync(url).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new RestQueryResponse
                   {
                       Content = content,
                       HttpStatusCode = response.StatusCode
                   };
        }

        public RestQueryResponse QueryPostFile(string url, string applicationId, string documentType, string instanceId, string fieldName, string fileName, Stream fileStream, string sessionId = null)
        {
            var linkedData = new
                             {
                                 InstanceId = instanceId,
                                 FieldName = fieldName,
                                 FileName = fileName,
                                 SessionId = sessionId,
                                 ApplicationId = applicationId,
                                 DocumentType = documentType
                             };

            var linkedDataJson = JsonConvert.SerializeObject(linkedData);

            var urlBuilder = new StringBuilder(url.Trim('/'));
            urlBuilder.Append($"/?linkedData={linkedDataJson}");

            var dataContent = new MultipartFormDataContent { { new StreamContent(fileStream), fileName, fileName } };

            var response = _client.PostAsync(urlBuilder.ToString(), dataContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new RestQueryResponse
                   {
                       Content = content,
                       HttpStatusCode = response.StatusCode
                   };
        }

        public RestQueryResponse QueryGetUrlEncodedData(string url, object formData)
        {
            var formDataJson = SerializeObjectToJson(formData);

            var urlBuilder = new StringBuilder(url.Trim('/'));
            urlBuilder.Append($"/?Form={formDataJson}");
            var response = _client.GetAsync(urlBuilder.ToString()).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new RestQueryResponse
                   {
                       Content = content,
                       HttpStatusCode = response.StatusCode
                   };
        }

        private static StringContent CreateJsonHttpContent(object body)
        {
            var jsonRequestBody = body != null
                ? SerializeObjectToJson(body)
                : string.Empty;

            var jsonRequestContent = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            return jsonRequestContent;
        }

        private static string SerializeObjectToJson(object target)
        {
            return target != null
                ? JsonConvert.SerializeObject(target)
                : null;
        }
    }
}