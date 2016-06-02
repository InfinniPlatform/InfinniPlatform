using System.Collections.Generic;
using System.Linq;
using System.Net;

using EasyNetQ.Management.Client;
using EasyNetQ.Management.Client.Model;

using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.MessageQueue.RabbitMq.Connection
{
    /// <summary>
    /// Менеджер RabbitMQ, инкапсулирующий функции, доступные через RabbitMQ Management HTTP API.
    /// </summary>
    internal class RabbitMqHttpManager
    {
        public RabbitMqHttpManager(RabbitMqConnectionSettings connectionSettings,
                                   ILog log)
        {
            _log = log;

            _managementClient = new ManagementClient($"http://{connectionSettings.HostName}", connectionSettings.UserName, connectionSettings.Password, connectionSettings.ManagementApiPort);
            try
            {
                _managementClient.GetConnections();
            }
            catch (WebException e)
            {
                _log.Error(e);
            }
        }

        private readonly ILog _log;
        private readonly ManagementClient _managementClient;

        /// <summary>
        /// Получить список очередей.
        /// </summary>
        public IEnumerable<Queue> GetQueues()
        {
            try
            {
                return _managementClient.GetQueues();
            }
            catch (WebException e)
            {
                _log.Error(e);
            }

            return Enumerable.Empty<Queue>();
        }

        /// <summary>
        /// Удаляет очереди из списка.
        /// </summary>
        /// <param name="queues">Список очередей.</param>
        public void DeleteQueues(IEnumerable<Queue> queues)
        {
            foreach (var q in queues.Where(queue => queue.AutoDelete != true))
            {
                try
                {
                    _managementClient.DeleteQueue(q);
                }
                catch (WebException e)
                {
                    _log.Error(e);
                }
            }
        }

        /// <summary>
        /// Возвращает список связей между точками обмена и очередью.
        /// </summary>
        public IEnumerable<Binding> GetBindings()
        {
            try
            {
                return _managementClient.GetBindings();
            }
            catch (WebException e)
            {
                _log.Error(e);
            }

            return Enumerable.Empty<Binding>();
        }

        /// <summary>
        /// Возвращает список точек обмена.
        /// </summary>
        public IEnumerable<Exchange> GetExchanges()
        {
            try
            {
                return _managementClient.GetExchanges();
            }
            catch (WebException e)
            {
                _log.Error(e);
            }

            return Enumerable.Empty<Exchange>();
        }

        /// <summary>
        /// Удаляет все сообщения из очереди.
        /// </summary>
        /// <param name="queue">Очередь.</param>
        public void PurgeQueue(Queue queue)
        {
            try
            {
                _managementClient.Purge(queue);
            }
            catch (WebException e)
            {
                _log.Error(e);
            }
        }
    }
}