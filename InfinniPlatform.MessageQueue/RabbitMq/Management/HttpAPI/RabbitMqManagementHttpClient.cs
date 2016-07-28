using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.MessageQueue.RabbitMq.Management.HttpAPI
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

        public RabbitMqManagementHttpClient(RabbitMqConnectionSettings connectionSettings)
        {
            var httpMessageHandler = new HttpClientHandler { Credentials = new NetworkCredential(connectionSettings.UserName, connectionSettings.Password) };
            _httpClient = new HttpClient(httpMessageHandler) { BaseAddress = new Uri($"http://{connectionSettings.HostName}:{connectionSettings.ManagementApiPort}") };

            var connections = Task.Run(() => Get<IEnumerable<Connection>>("/api/connections")).Result;
            if (connections == null)
            {
                throw new AggregateException("Cannot connect to RabbitMQ Management HTTP API.");
            }
        }

        private readonly HttpClient _httpClient;

        /// <summary>
        /// Получить список очередей.
        /// </summary>
        public async Task<IEnumerable<Queue>> GetQueues()
        {
            return await Get<IEnumerable<Queue>>("/api/queues");
        }

        /// <summary>
        /// Удаляет очереди из списка.
        /// </summary>
        /// <param name="queues">Список очередей.</param>
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
            return await Get<IEnumerable<Binding>>("/api/bindings");
        }

        /// <summary>
        /// Возвращает список точек обмена.
        /// </summary>
        public async Task<IEnumerable<Exchange>> GetExchanges()
        {
            return await Get<IEnumerable<Exchange>>("/api/exchanges");
        }

        /// <summary>
        /// Удаляет все сообщения из очереди.
        /// </summary>
        /// <param name="queue">Очередь.</param>
        public async Task PurgeQueue(Queue queue)
        {
            await Delete($"/api/queues/{DefaultVhost}/{queue.Name}/contents");
        }

        private async Task<T> Get<T>(string relativeUrl)
        {
            var httpResponseMessage = await _httpClient.GetAsync(new Uri(relativeUrl, UriKind.Relative));
            var content = await httpResponseMessage.Content.ReadAsStringAsync();

            return JsonObjectSerializer.Default.Deserialize<T>(content);
        }

        private async Task Delete(string relativeUrl)
        {
            await _httpClient.DeleteAsync(new Uri(relativeUrl, UriKind.Relative));
        }
    }
}