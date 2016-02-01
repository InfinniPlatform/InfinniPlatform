using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// Вспомогательный класс для работы с HTTP.
    /// </summary>
    public sealed class RequestExecutor
    {
        [ThreadStatic]
        private static RequestExecutor _instance;

        public static readonly RequestExecutor Instance = new RequestExecutor();

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

        public Stream GetDownload(string uri)
        {
            var response = _client.GetAsync(uri).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(response.ReasonPhrase);
            }

            return response.Content.ReadAsStreamAsync().Result;
        }

        public Stream PostDownload(string uri, object content = null)
        {
            var jsonContent = CreateJsonContent(content);

            var response = _client.PostAsync(uri, jsonContent).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(response.ReasonPhrase);
            }

            return response.Content.ReadAsStreamAsync().Result;
        }

        public object PostObject(string uri, object content = null)
        {
            var jsonContent = CreateJsonContent(content);

            var response = _client.PostAsync(uri, jsonContent).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(result);
            }

            return DeserializeObject(result);
        }

        public IEnumerable<object> PostArray(string uri, object content = null)
        {
            var jsonContent = CreateJsonContent(content);

            var response = _client.PostAsync(uri, jsonContent).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(result);
            }

            return DeserializeArray(result);
        }

        public void PostFile(string uri, string fileName, Stream fileContent)
        {
            var streamContent = new MultipartFormDataContent { { new StreamContent(fileContent), fileName, fileName } };

            var response = _client.PostAsync(uri, streamContent).Result;

            if (!response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;

                throw new InvalidOperationException(result);
            }
        }

        private static StringContent CreateJsonContent(object body)
        {
            var bodyString = SerializeObject(body);

            return new StringContent(bodyString ?? string.Empty, Encoding.UTF8, HttpConstants.JsonContentType);
        }

        private static string SerializeObject(object target)
        {
            return JsonObjectSerializer.Default.ConvertToString(target);
        }

        private static object DeserializeObject(string value)
        {
            return JsonObjectSerializer.Default.Deserialize<DynamicWrapper>(value);
        }

        private static IEnumerable<object> DeserializeArray(string value)
        {
            return JsonObjectSerializer.Default.Deserialize<DynamicWrapper[]>(value);
        }
    }
}