using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

using InfinniPlatform.Sdk.Properties;

using Newtonsoft.Json;

namespace InfinniPlatform.Sdk
{
    //TODO Класс по сути дублирует RequestExecutor в сборке InfinniPlatform.Api. Необходимо избавиться от дублирования.
    public sealed class RequestExecutor
    {
        [ThreadStatic]
        private static RequestExecutor _instance;

        public RequestExecutor()
        {
            var clientHandler = new HttpClientHandler { CookieContainer = new CookieContainer() };
            var client = new HttpClient(clientHandler);

            _client = client;
        }

        private readonly HttpClient _client;

        public static RequestExecutor Instance => _instance ?? (_instance = new RequestExecutor());

        public string QueryGet(string url, string arguments = null)
        {
            var urlBuilder = new StringBuilder(url);

            if (!string.IsNullOrEmpty(arguments))
            {
                urlBuilder.Append($"?{arguments}");
            }

            var response = _client.GetAsync(urlBuilder.ToString()).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(string.Format(Resources.CannotProcessGetRequest, response.StatusCode, content));
            }

            return content;
        }

        public string QueryPost(string url, object body = null)
        {
            var jsonRequestContent = CreateJsonHttpContent(body);

            var response = _client.PostAsync(new Uri(url), jsonRequestContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(string.Format(Resources.CannotProcessPostRequest, response.StatusCode, content));
            }

            return content;
        }

        /// <summary>
        /// Формирование запроса на вставку нового документа
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="body">Тело запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public string QueryPut(string url, object body = null)
        {
            var jsonRequestContent = CreateJsonHttpContent(body);

            var response = _client.PutAsync(new Uri(url), jsonRequestContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(string.Format(Resources.CannotProcessPutRequest, response.StatusCode, content));
            }

            return content;
        }

        public string QueryDelete(string url, dynamic body = null)
        {
            var response = _client.DeleteAsync(new Uri(url)).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(string.Format(Resources.CannotProcessDeleteRequest, response.StatusCode, content));
            }

            return content;
        }

        public string QueryGetById(string url)
        {
            var response = _client.GetAsync(new Uri(url)).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(string.Format(Resources.CannotProcessGetRequest, response.StatusCode, content));
            }

            return content;
        }

        public string QueryPostFile(string url, string applicationId, string documentType, string instanceId, string fieldName, string fileName, Stream fileStream, string sessionId = null)
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

            var urlBuilder = new StringBuilder(url);
            urlBuilder.Append($"?linkedData={linkedDataJson}");

            var dataContent = new MultipartFormDataContent { { new StreamContent(fileStream), fileName, fileName } };

            var response = _client.PostAsync(new Uri(urlBuilder.ToString()), dataContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(string.Format(Resources.CannotProcessPostRequest, response.StatusCode, content));
            }

            return content;
        }

        public string QueryGetUrlEncodedData(string url, object formData)
        {
            var formDataJson = SerializeObjectToJson(formData);

            var urlBuilder = new StringBuilder(url);
            urlBuilder.Append($"?Form={formDataJson}");
            var response = _client.GetAsync(new Uri(url)).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(string.Format(Resources.CannotProcessGetRequest, response.StatusCode, content));
            }

            return content;
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