using System;
using System.Collections.Generic;
using System.Linq;

using EasyNetQ.Management.Client;
using EasyNetQ.Management.Client.Model;

using RabbitMQ.Client;

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

        public IConnection GetConnection()
        {
            return _connection.Value;
        }

        /// <summary>
        /// Создает канал.
        /// </summary>
        public IModel GetChannel()
        {
            var channel = _connection.Value.CreateModel();
            channel.BasicQos(0, 1, false);

            return channel;
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

        public IEnumerable<Queue> GetQueues()
        {
            return _managementClient.GetQueues();
        }

        public void DeleteQueues(IEnumerable<Queue> queues)
        {
            foreach (var q in queues.Where(queue => queue.AutoDelete != true))
            {
                _managementClient.DeleteQueue(q);
            }
        }

        public IEnumerable<Binding> GetBindings()
        {
            return _managementClient.GetBindings();
        }

        public IEnumerable<Exchange> GetExchanges()
        {
            return _managementClient.GetExchanges();
        }
    }
}