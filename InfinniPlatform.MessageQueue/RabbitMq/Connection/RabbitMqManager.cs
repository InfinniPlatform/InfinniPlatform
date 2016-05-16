using System;
using System.Collections.Generic;

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
//                                                    model.ExchangeDeclare($"{applicationName}.{Defaults.Exchange.Type.Fanout}", Defaults.Exchange.Type.Fanout, Defaults.Exchange.Durable, Defaults.Exchange.AutoDelete, null);
//                                                    _exchangeNames = new Dictionary<string, string>
//                                                                     {
//                                                                         { Defaults.Exchange.Type.Direct, $"{applicationName}.{Defaults.Exchange.Type.Direct}" },
//                                                                         { Defaults.Exchange.Type.Topic, $"{applicationName}.{Defaults.Exchange.Type.Topic}" },
//                                                                         { Defaults.Exchange.Type.Fanout, $"{applicationName}.{Defaults.Exchange.Type.Fanout}" }
//                                                                     };

                                                    model.Close();

                                                    return connection;
                                                });
        }

        private readonly Lazy<IConnection> _connection;
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
            channel.BasicQos(0,1,false);

            return channel;
        }

        /// <summary>
        /// Создает очередь, если она еще не существует.
        /// </summary>
        /// <param name="queueKey">Ключ/имя очереди.</param>
        /// <returns></returns>
        public void DeclareTaskQueue(string queueKey)
        {
            var channel = GetChannel();
            channel.QueueDeclare(queueKey, Defaults.Queue.Durable, Defaults.Queue.Exclusive, Defaults.Queue.AutoDelete, null);
//            channel.QueueBind(queueKey, _exchangeNames[ExchangeType.Direct], queueKey);
        }
    }
}