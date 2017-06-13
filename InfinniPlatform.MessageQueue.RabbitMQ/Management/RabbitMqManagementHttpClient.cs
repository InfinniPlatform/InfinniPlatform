using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using InfinniPlatform.Dynamic;
using InfinniPlatform.Serialization;

namespace InfinniPlatform.MessageQueue.Management
{
    /// <summary>
    /// Менеджер RabbitMQ, инкапсулирующий функции, доступные через RabbitMQ Management HTTP API.
    /// </summary>
    internal class RabbitMqManagementHttpClient
    {
        /// <summary>
        /// Виртуальный хост по умолчанию ('/').
        /// </summary>
        private const string DefaultVhost = "%2f";

        private readonly Lazy<HttpClient> _httpClient;
        private readonly IJsonObjectSerializer _serializer;

        public RabbitMqManagementHttpClient(RabbitMqMessageQueueOptions options, IJsonObjectSerializer serializer)
        {
            _httpClient = new Lazy<HttpClient>(() => GetHttpClient(options));
            _serializer = serializer;
        }

        private static HttpClient GetHttpClient(RabbitMqMessageQueueOptions options)
        {
            var httpMessageHandler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(options.UserName, options.Password)
            };

            var httpClient = new HttpClient(httpMessageHandler)
            {
                BaseAddress = new Uri($"http://{options.HostName}:{options.ManagementApiPort}")
            };

            try
            {
                var httpResponseMessage = httpClient.GetAsync("/").Result;
            }
            catch (Exception e)
            {
                throw new AggregateException("RabbitMQ Management plugin must be enabled to run tests.", e);
            }

            return httpClient;
        }

        public async Task<DynamicDocument> GetOverview()
        {
            return await Get("/api/overview");
        }

        /// <summary>
        /// Получить список очередей.
        /// </summary>
        public async Task<IEnumerable<Queue>> GetQueues()
        {
            return await Get<Queue>("/api/queues");
        }

        /// <summary>
        /// Удаляет очереди из списка.
        /// </summary>
        /// <param name="queues">Список очередей.</param>
        [Obsolete("Returns method not allowed.")]
        public async Task DeleteQueues(IEnumerable<Queue> queues)
        {
            foreach (var q in queues.Where(queue => queue.AutoDelete != true))
            {
                await Delete($"/api/queues/{DefaultVhost}/{q.Name}");
            }
        }

        /// <summary>
        /// Возвращает список связей между точками обмена и очередью.
        /// </summary>
        public async Task<IEnumerable<Binding>> GetBindings()
        {
            return await Get<Binding>("/api/bindings");
        }

        /// <summary>
        /// Возвращает список точек обмена.
        /// </summary>
        public async Task<IEnumerable<Exchange>> GetExchanges()
        {
            return await Get<Exchange>("/api/exchanges");
        }

        /// <summary>
        /// Удаляет все сообщения из очереди.
        /// </summary>
        /// <param name="queue">Очередь.</param>
        public async Task PurgeQueue(Queue queue)
        {
            await Delete($"/api/queues/{DefaultVhost}/{queue.Name}/contents");
        }

        private async Task<DynamicDocument> Get(string relativeUrl)
        {
            var httpResponseMessage = await _httpClient.Value.GetAsync(new Uri(relativeUrl, UriKind.Relative));
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            return _serializer.Deserialize<DynamicDocument>(content);
        }

        private async Task<IEnumerable<T>> Get<T>(string relativeUrl)
        {
            var httpResponseMessage = await _httpClient.Value.GetAsync(new Uri(relativeUrl, UriKind.Relative));
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            return _serializer.Deserialize<IEnumerable<T>>(content);
        }

        private async Task Delete(string relativeUrl)
        {
            var requestUri = new Uri(relativeUrl, UriKind.Relative);
            var response = await _httpClient.Value.DeleteAsync(requestUri);
            await response.Content.ReadAsStringAsync();
        }
    }
}