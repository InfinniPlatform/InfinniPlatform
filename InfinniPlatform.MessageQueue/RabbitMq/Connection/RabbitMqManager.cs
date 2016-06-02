using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using EasyNetQ.Management.Client;
using EasyNetQ.Management.Client.Model;

using InfinniPlatform.Sdk.Settings;

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace InfinniPlatform.MessageQueue.RabbitMq.Connection
{
    internal sealed class RabbitMqManager
    {
        public RabbitMqManager(RabbitMqConnectionSettings connectionSettings, IAppEnvironment appEnvironment)
        {
            BroadcastExchangeName = $"{appEnvironment.Name}.{Defaults.Exchange.Type.Fanout}";

            _connection = new Lazy<IConnection>(() =>
                                                {
                                                    var connectionFactory = new ConnectionFactory
                                                                            {
                                                                                HostName = connectionSettings.HostName,
                                                                                Port = connectionSettings.Port,
                                                                                UserName = connectionSettings.UserName,
                                                                                Password = connectionSettings.Password,
                                                                                AutomaticRecoveryEnabled = Defaults.Connection.AutomaticRecoveryEnabled
                                                                            };

                                                    var connection = connectionFactory.CreateConnection();
                                                    var channel = connection.CreateModel();
                                                    channel.ExchangeDeclare(BroadcastExchangeName, Defaults.Exchange.Type.Fanout, Defaults.Exchange.Durable, Defaults.Exchange.AutoDelete, null);
                                                    channel.Close();

                                                    return connection;
                                                });

            _managementClient = new ManagementClient($"http://{connectionSettings.HostName}", connectionSettings.UserName, connectionSettings.Password, connectionSettings.ManagementApiPort);
        }

        private readonly Lazy<IConnection> _connection;
        private readonly ManagementClient _managementClient;

        public string BroadcastExchangeName { get; }

        /// <summary>
        /// Возвращает абстракцию соединения с RabbitMq.
        /// </summary>
        public IConnection GetConnection()
        {
            return _connection.Value;
        }

        /// <summary>
        /// Создает канал.
        /// </summary>
        public IModel GetChannel()
        {
            var alreadyClosedException = new AlreadyClosedException(new ShutdownEventArgs(ShutdownInitiator.Application, 200, "Unable to create channel."));
            for (var i = 0; i < 10; i++)
            {
                try
                {
                    var channel = _connection.Value.CreateModel();
                    channel.BasicQos(0, 1, false);

                    return channel;
                }
                catch (AlreadyClosedException e)
                {
                    alreadyClosedException = e;
                    Thread.Sleep(1000);
                }
            }
            throw alreadyClosedException;
        }

        /// <summary>
        /// Создает очередь для сообщений по ключу.
        /// </summary>
        /// <param name="queueKey">Ключ/имя очереди.</param>
        public void DeclareTaskQueue(string queueKey)
        {
            var channel = GetChannel();
            channel.QueueDeclare(queueKey, Defaults.Queue.Durable, Defaults.Queue.Exclusive, Defaults.Queue.AutoDelete, null);
        }

        /// <summary>
        /// Создает очередь для широковещательных сообщений.
        /// </summary>
        public string DeclareBroadcastQueue(string routingKey)
        {
            var channel = GetChannel();
            var queueName = channel.QueueDeclare().QueueName;

            channel.QueueBind(queueName, BroadcastExchangeName, routingKey);

            return queueName;
        }

        /// <summary>
        /// Получить список очередей.
        /// </summary>
        public IEnumerable<Queue> GetQueues()
        {
            return _managementClient.GetQueues();
        }

        /// <summary>
        /// Удаляет очереди из списка.
        /// </summary>
        /// <param name="queues">Список очередей.</param>
        public void DeleteQueues(IEnumerable<Queue> queues)
        {
            foreach (var q in queues.Where(queue => queue.AutoDelete != true))
            {
                _managementClient.DeleteQueue(q);
            }
        }

        /// <summary>
        /// Возвращает список связей между точками обмена и очередью.
        /// </summary>
        public IEnumerable<Binding> GetBindings()
        {
            return _managementClient.GetBindings();
        }

        /// <summary>
        /// Возвращает список точек обмена.
        /// </summary>
        public IEnumerable<Exchange> GetExchanges()
        {
            return _managementClient.GetExchanges();
        }

        /// <summary>
        /// Удаляет все сообщения из очереди.
        /// </summary>
        /// <param name="queue">Очередь.</param>
        public void Get(Queue queue)
        {
            _managementClient.Purge(queue);
        }

        public bool IsAlive()
        {
            var defaultVhost = new Vhost { Name = "/" };

            try
            {
                var isAlive = _managementClient.IsAlive(defaultVhost);
                return isAlive;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}