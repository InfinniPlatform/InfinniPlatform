using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Infinni.Server.HttpService;
using Infinni.Server.Properties;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Serialization;

namespace Infinni.Server.Agent
{
    /// <summary>
    /// Клиент для HTTP-сервиса приложения.
    /// </summary>
    public class AgentHttpClient
    {
        public AgentHttpClient(IJsonObjectSerializer serializer)
        {
            _serializer = serializer;
            _httpClient = new HttpClient();
        }

        private readonly HttpClient _httpClient;
        private readonly IJsonObjectSerializer _serializer;

        /// <summary>
        /// Отправляет GET-запрос.
        /// </summary>
        /// <typeparam name="T">Тип объекта, получаемого в ответе.</typeparam>
        /// <param name="path">Путь запроса.</param>
        /// <param name="address">Адрес агента.</param>
        /// <param name="port">Порт агента.</param>
        /// <param name="queryContent">Параметры запроса.</param>
        public async Task<T> Get<T>(string path, string address, int port, DynamicWrapper queryContent = null)
        {
            var uriString = $"http://{address}:{port}/agent/{path}{ToQuery(queryContent)}";

            var response = await _httpClient.GetAsync(uriString);

            CheckResponse(response);

            var content = await response.Content.ReadAsStreamAsync();

            return _serializer.Deserialize<T>(content);
        }

        /// <summary>
        /// Отправляет GET-запрос.
        /// </summary>
        /// <typeparam name="T">Тип объекта, получаемого в ответе.</typeparam>
        /// <param name="path">Путь запроса.</param>
        /// <param name="address">Адрес агента.</param>
        /// <param name="port">Порт агента.</param>
        /// <param name="formContent">Содержимое формы запроса.</param>
        public async Task<T> Post<T>(string path, string address, int port, DynamicWrapper formContent)
        {
            var uriString = $"http://{address}:{port}/agent/{path}";

            var formStringContent = _serializer.ConvertToString(formContent);
            var requestContent = new StringContent(formStringContent, _serializer.Encoding, HttpConstants.JsonContentType);

            var response = await _httpClient.PostAsync(new Uri(uriString), requestContent);

            CheckResponse(response);

            var content = await response.Content.ReadAsStreamAsync();

            return _serializer.Deserialize<T>(content);
        }

        /// <summary>
        /// Отправляет GET-запрос на получение файлового потока.
        /// </summary>
        /// <param name="path">Путь запроса.</param>
        /// <param name="address">Адрес агента.</param>
        /// <param name="port">Порт агента.</param>
        /// <param name="queryContent">Параметры запроса.</param>
        public async Task<Stream> GetStream(string path, string address, int port, DynamicWrapper queryContent = null)
        {
            var uriString = $"http://{address}:{port}/agent/{path}{ToQuery(queryContent)}";

            var response = await _httpClient.GetAsync(uriString);
            var content = await response.Content.ReadAsStreamAsync();

            return content;
        }


        private static void CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new HttpServiceException(Resources.UnableConnectAgent);
                }
            }
        }

        private static string ToQuery(DynamicWrapper queryContent)
        {
            return queryContent?.ToDictionary().Aggregate("?", (current, pair) => current + $"{pair.Key}={pair.Value}&")?.TrimEnd('&');
        }
    }
}