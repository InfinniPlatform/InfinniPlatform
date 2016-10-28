using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// Вспомогательный класс для работы с HTTP.
    /// </summary>
    public sealed class RequestExecutor
    {
        [ThreadStatic]
        private static HttpClient _client;
        private static readonly object ClientSync = new object();
        private static readonly HttpContent EmptyHttpContent = new StringContent("{}", Encoding.UTF8, HttpConstants.JsonContentType);


        private static HttpClient Client
        {
            get
            {
                if (_client == null)
                {
                    lock (ClientSync)
                    {
                        if (_client == null)
                        {
                            var clientHandler = new HttpClientHandler { CookieContainer = new CookieContainer() };
                            var client = new HttpClient(clientHandler);

#if DEBUG
                            client.Timeout = Timeout.InfiniteTimeSpan;
#endif

                            _client = client;
                        }
                    }
                }

                return _client;
            }
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
        public async Task<Stream> GetStreamAsync(string requestUri)
        {
            var response = await Client.GetAsync(requestUri);
            var stream = response.IsSuccessStatusCode ? await response.Content.ReadAsStreamAsync() : null;
            return stream;
        }


        public async Task<T> PostAsync<T>(string requestUri, HttpContent requestContent = null)
        {
            var response = await Client.PostAsync(requestUri, requestContent ?? EmptyHttpContent);
            var stream = await response.Content.ReadAsStreamAsync();
            return _serializer.Deserialize<T>(stream);
        }

        public async Task<Stream> PostStreamAsync(string requestUri, HttpContent requestContent = null)
        {
            var response = await Client.PostAsync(requestUri, requestContent ?? EmptyHttpContent);
            var stream = response.IsSuccessStatusCode ? await response.Content.ReadAsStreamAsync() : null;
            return stream;
        }


        public async Task<T> DeleteAsync<T>(string requestUri)
        {
            var response = await Client.DeleteAsync(requestUri);
            var stream = await response.Content.ReadAsStreamAsync();
            return _serializer.Deserialize<T>(stream);
        }
    }
}