using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using EasyNetQ.Management.Client;
using EasyNetQ.Management.Client.Model;

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace InfinniPlatform.MessageQueue.RabbitMq.Connection
{
    internal sealed class RabbitMqManager
    {
        public RabbitMqManager(RabbitMqConnectionSettings connectionSettings, string applicationName)
        {
            var connectionFactory = new ConnectionFactory
                                    {
                                        HostName = connectionSettings.HostName,
                                        Port = connectionSettings.Port,
                                        UserName = connectionSettings.UserName,
                                        Password = connectionSettings.Password,
                                        AutomaticRecoveryEnabled = Defaults.Connection.AutomaticRecoveryEnabled
                                    };

            _connection = new Lazy<IConnection>(() =>
                                                {
                                                    var connection = connectionFactory.CreateConnection();
                                                    var model = connection.CreateModel();

                                                    //                                                    model.ExchangeDeclare($"{applicationName}.{Defaults.Exchange.Type.Direct}", Defaults.Exchange.Type.Direct, Defaults.Exchange.Durable, Defaults.Exchange.AutoDelete, null);
                                                    //                                                    model.ExchangeDeclare($"{applicationName}.{Defaults.Exchange.Type.Topic}", Defaults.Exchange.Type.Topic, Defaults.Exchange.Durable, Defaults.Exchange.AutoDelete, null);
                                                    model.ExchangeDeclare($"{applicationName}.{Defaults.Exchange.Type.Fanout}", Defaults.Exchange.Type.Fanout, Defaults.Exchange.Durable, Defaults.Exchange.AutoDelete, null);
                                                    _exchangeNames = new Dictionary<string, string>
                                                                     {
                                                                         //                                                                         { Defaults.Exchange.Type.Direct, $"{applicationName}.{Defaults.Exchange.Type.Direct}" },
                                                                         //                                                                         { Defaults.Exchange.Type.Topic, $"{applicationName}.{Defaults.Exchange.Type.Topic}" },
                                                                         { Defaults.Exchange.Type.Fanout, $"{applicationName}.{Defaults.Exchange.Type.Fanout}" }
                                                                     };

                                                    model.Close();

                                                    return connection;
                                                });

            _managementClient = new ManagementClient($"http://{connectionSettings.HostName}", connectionSettings.UserName, connectionSettings.Password, connectionSettings.ManagementApiPort);
        }

        private readonly Lazy<IConnection> _connection;
        private readonly ManagementClient _managementClient;
        private Dictionary<string, string> _exchangeNames;

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
        public string DeclareFanoutQueue()
        {
            var channel = GetChannel();
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queueName, _exchangeNames[Defaults.Exchange.Type.Fanout], "");
            return queueName;
        }

        public string GetExchangeNameByType(string type)
        {
            return _exchangeNames[type];
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