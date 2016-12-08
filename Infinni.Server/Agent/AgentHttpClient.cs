using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Infinni.Server.HttpService;
using Infinni.Server.Properties;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;

using Newtonsoft.Json;

namespace Infinni.Server.Agent
{
    /// <summary>
    /// Клиент для HTTP-сервиса приложения.
    /// </summary>
    public class AgentHttpClient
    {
        private static readonly Dictionary<Type, Action> ExceptionsWrappers = new Dictionary<Type, Action>
                                                                              {
                                                                                  { typeof(UriFormatException), () => { throw new HttpServiceException(Resources.CheckUriFormat); } },
                                                                                  { typeof(HttpRequestException), () => { throw new HttpServiceException(Resources.UnableConnectAgent); } },
                                                                                  { typeof(JsonReaderException), () => { throw new HttpServiceException(Resources.UnableReadAgentResponse); } }
                                                                              };

        public AgentHttpClient(IJsonObjectSerializer serializer,
                               ILog log)
        {
            _serializer = serializer;
            _log = log;
            _httpClient = new HttpClient();
        }

        private readonly HttpClient _httpClient;
        private readonly ILog _log;
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

            var result = await TryExecute(async () =>
                                          {
                                              var response = await _httpClient.GetAsync(uriString);

                                              CheckResponse(response);

                                              var content = await response.Content.ReadAsStreamAsync();

                                              return _serializer.Deserialize<T>(content);
                                          });

            return result;
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

            var result = await TryExecute(async () =>
                                          {
                                              var formStringContent = _serializer.ConvertToString(formContent);
                                              var requestContent = new StringContent(formStringContent, _serializer.Encoding, HttpConstants.JsonContentType);

                                              var response = await _httpClient.PostAsync(new Uri(uriString), requestContent);

                                              CheckResponse(response);

                                              var content = await response.Content.ReadAsStreamAsync();

                                              return _serializer.Deserialize<T>(content);
                                          });

            return result;
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

        private async Task<T> TryExecute<T>(Func<Task<T>> request)
        {
            try
            {
                return await request.Invoke();
            }
            catch (Exception e)
            {
                _log.Error(e);

                var exceptionType = e.GetType();

                if (ExceptionsWrappers.ContainsKey(exceptionType))
                {
                    ExceptionsWrappers[exceptionType].Invoke();
                }

                throw new HttpServiceException(Resources.UnexpectedServerError);
            }
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