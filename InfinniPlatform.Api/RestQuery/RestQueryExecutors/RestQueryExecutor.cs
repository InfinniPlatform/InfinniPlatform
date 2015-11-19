using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

namespace InfinniPlatform.Api.RestQuery.RestQueryExecutors
{
    //TODO Класс по сути дублирует RequestExecutor в сборке InfinniPlatform.Sdk. Необходимо избавиться от дублирования.
    /// <summary>
    /// Сервис для отправки HTTP запросов.
    /// </summary>
    public sealed class RestQueryExecutor : IRestQueryExecutor
    {
        /// <summary>
        /// Сервис для отправки HTTP запросов.
        /// </summary>
        public RestQueryExecutor()
        {
            var clientHandler = new HttpClientHandler { CookieContainer = new CookieContainer() };
            var client = new HttpClient(clientHandler);

            _client = client;
        }

        private readonly HttpClient _client;

        /// <summary>
        /// Отправляет GET-запрос.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="queryObject"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public RestQueryResponse QueryGet(string url, object queryObject)
        {
            var argumentsString = JsonConvert.SerializeObject(queryObject);

            var urlBuilder = new StringBuilder(url);

            if (argumentsString != null)
            {
                urlBuilder.Append($"?query={argumentsString}");
            }

            var response = _client.GetAsync(urlBuilder.ToString()).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new RestQueryResponse
                   {
                       Content = content,
                       HttpStatusCode = response.StatusCode
                   };
        }

        /// <summary>
        /// Отправляет POST-запрос.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="body"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public RestQueryResponse QueryPost(string url, object body)
        {
            var jsonRequestContent = body.ToJsonHttpContent();

            var response = _client.PostAsync(new Uri(url), jsonRequestContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new RestQueryResponse
                   {
                       Content = content,
                       HttpStatusCode = response.StatusCode
                   };
        }

        /// <summary>
        /// Отправляет POST-запрос с файлом в качестве параметра.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="linkedData">Параметры запроса.</param>
        /// <param name="filePath">Путь до файла.</param>
        public RestQueryResponse QueryPostFile(string url, object linkedData, string filePath)
        {
            var fileStream = new FileStream(filePath, FileMode.Open);
            return QueryPostFile(url, linkedData, fileStream);
        }

        /// <summary>
        /// Отправляет POST-запрос с параметрами.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="formData">Параметры.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public RestQueryResponse QueryPostUrlEncodedData(string url, object formData)
        {
            var formDataJson = formData.ToJsonHttpContent();

            var urlBuilder = new StringBuilder(url);
            urlBuilder.Append($"?Form={formDataJson}");
            var response = _client.PostAsync(new Uri(url), formDataJson).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new RestQueryResponse
                   {
                       Content = content,
                       HttpStatusCode = response.StatusCode
                   };
        }

        /// <summary>
        /// Отправляет GET-запрос с параметрами.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="formData">Параметры.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public RestQueryResponse QueryGetUrlEncodedData(string url, object formData)
        {
            var formDataJson = formData.SerializeToJson();

            var urlBuilder = new StringBuilder(url);
            urlBuilder.Append($"?Form={formDataJson}");
            var response = _client.GetAsync(new Uri(url)).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new RestQueryResponse
                   {
                       Content = content,
                       HttpStatusCode = response.StatusCode
                   };
        }

        /// <summary>
        /// Отправляет POST-запрос с файлом в качестве параметра.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="linkedData">Параметры запроса.</param>
        /// <param name="fileStream">Файловый поток.</param>
        /// <exception cref="InvalidOperationException"></exception>
        public RestQueryResponse QueryPostFile(string url, object linkedData, Stream fileStream)
        {
            string fileName = ((dynamic)linkedData).FileName.ToString();

            var linkedDataJson = JsonConvert.SerializeObject(linkedData);

            var urlBuilder = new StringBuilder(url);
            urlBuilder.Append($"?linkedData={linkedDataJson}");

            var dataContent = new MultipartFormDataContent { { new StreamContent(fileStream), fileName, fileName } };

            var response = _client.PostAsync(new Uri(urlBuilder.ToString()), dataContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;

            return new RestQueryResponse
                   {
                       Content = content,
                       HttpStatusCode = response.StatusCode
                   };
        }
    }


    /// <summary>
    /// Класс расширений для RequestExecutor.
    /// </summary>
    public static class RequestExecutorExtension
    {
        /// <summary>
        /// Преобразует объект в JSON-строку для передачи в HTTP-запросе.
        /// </summary>
        /// <param name="body">Объект.</param>
        public static StringContent ToJsonHttpContent(this object body)
        {
            var jsonBody = body != null
                               ? SerializeToJson(body)
                               : string.Empty;

            var jsonRequestContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            return jsonRequestContent;
        }

        /// <summary>
        /// Сериализует объект в JSON строку.
        /// </summary>
        /// <param name="target">Объект.</param>
        public static string SerializeToJson(this object target)
        {
            return target != null
                       ? JsonConvert.SerializeObject(target)
                       : null;
        }
    }
}