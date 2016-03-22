using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
        private static readonly HttpClient Client;


        static RequestExecutor()
        {
            var clientHandler = new HttpClientHandler { CookieContainer = new CookieContainer() };
            var client = new HttpClient(clientHandler);

#if DEBUG
            client.Timeout = Timeout.InfiniteTimeSpan;
#endif

            Client = client;
        }


        public RequestExecutor(IJsonObjectSerializer serializer)
        {
            _serializer = serializer;
        }


        private readonly IJsonObjectSerializer _serializer;


        public async Task<T> GetAsync<T>(string requestUri)
        {
            var response = await Client.GetAsync(requestUri);
            var stream = await response.Content.ReadAsStreamAsync();
            return _serializer.Deserialize<T>(stream);
        }

        public async Task<T> PostAsync<T>(string requestUri, MultipartFormDataContent requestContent)
        {
            var response = await Client.PostAsync(requestUri, requestContent);
            var stream = await response.Content.ReadAsStreamAsync();
            return _serializer.Deserialize<T>(stream);
        }

        public async Task<T> DeleteAsync<T>(string requestUri)
        {
            var response = await Client.DeleteAsync(requestUri);
            var stream = await response.Content.ReadAsStreamAsync();
            return _serializer.Deserialize<T>(stream);
        }


        [Obsolete]
        public Stream GetDownload(string uri)
        {
            var response = Client.GetAsync(uri).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(response.ReasonPhrase);
            }

            return response.Content.ReadAsStreamAsync().Result;
        }

        [Obsolete]
        public Stream PostDownload(string uri, object content = null)
        {
            var jsonContent = CreateJsonContent(content);

            var response = Client.PostAsync(uri, jsonContent).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(response.ReasonPhrase);
            }

            return response.Content.ReadAsStreamAsync().Result;
        }

        [Obsolete]
        public object PostObject(string uri, object content = null)
        {
            var jsonContent = CreateJsonContent(content);

            var response = Client.PostAsync(uri, jsonContent).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(result);
            }

            return DeserializeObject(result);
        }

        [Obsolete]
        public IEnumerable<object> PostArray(string uri, object content = null)
        {
            var jsonContent = CreateJsonContent(content);

            var response = Client.PostAsync(uri, jsonContent).Result;

            var result = response.Content.ReadAsStringAsync().Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(result);
            }

            return DeserializeArray(result);
        }

        [Obsolete]
        public void PostFile(string uri, string fileName, string fileType, Stream fileContent)
        {
            var streamContent = new StreamContent(fileContent);

            if (!string.IsNullOrEmpty(fileType))
            {
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(fileType);
            }

            var response = Client.PostAsync(uri, new MultipartFormDataContent { { streamContent, "\"File\"", $"\"{fileName}\"" } }).Result;

            if (!response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;

                throw new InvalidOperationException(result);
            }
        }


        [Obsolete]
        private StringContent CreateJsonContent(object body)
        {
            var bodyString = SerializeObject(body);

            return new StringContent(bodyString ?? string.Empty, Encoding.UTF8, HttpConstants.JsonContentType);
        }

        [Obsolete]
        private string SerializeObject(object target)
        {
            return _serializer.ConvertToString(target);
        }

        [Obsolete]
        private object DeserializeObject(string value)
        {
            return _serializer.Deserialize<DynamicWrapper>(value);
        }

        [Obsolete]
        private IEnumerable<object> DeserializeArray(string value)
        {
            return _serializer.Deserialize<DynamicWrapper[]>(value);
        }
    }
}