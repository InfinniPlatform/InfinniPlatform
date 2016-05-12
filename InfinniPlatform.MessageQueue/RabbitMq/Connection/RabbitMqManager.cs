using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Settings;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMq.Connection
{
    internal sealed class RabbitMqManager
    {
        public RabbitMqManager(RabbitMqConnectionSettings connectionSettings, IAppEnvironment appSettings)
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

                                                    model.ExchangeDeclare($"{appSettings.Name}.{Defaults.Exchange.Type.Direct}", Defaults.Exchange.Type.Direct, Defaults.Exchange.Durable, Defaults.Exchange.AutoDelete, null);
                                                    model.ExchangeDeclare($"{appSettings.Name}.{Defaults.Exchange.Type.Topic}", Defaults.Exchange.Type.Topic, Defaults.Exchange.Durable, Defaults.Exchange.AutoDelete, null);
                                                    model.ExchangeDeclare($"{appSettings.Name}.{Defaults.Exchange.Type.Fanout}", Defaults.Exchange.Type.Fanout, Defaults.Exchange.Durable, Defaults.Exchange.AutoDelete, null);
                                                    _exchangeNames = new Dictionary<string, string>
                                                                     {
                                                                         { Defaults.Exchange.Type.Direct, $"{appSettings.Name}.{Defaults.Exchange.Type.Direct}" },
                                                                         { Defaults.Exchange.Type.Topic, $"{appSettings.Name}.{Defaults.Exchange.Type.Topic}" },
                                                                         { Defaults.Exchange.Type.Fanout, $"{appSettings.Name}.{Defaults.Exchange.Type.Fanout}" }
                                                                     };

                                                    model.Close();

                                                    return connection;
                                                });
        }

        private readonly Dictionary<string, IModel> _channels = new Dictionary<string, IModel>();

        private readonly Lazy<IConnection> _connection;
        private readonly ConcurrentDictionary<string, string> _queues = new ConcurrentDictionary<string, string>();
        private Dictionary<string, string> _exchangeNames;

        public IConnection GetConnection()
        {
            return _connection.Value;
        }

        /// <summary>
        /// Создает канал, если он еще не существует.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IModel GetChannel(string key)
        {
            if (_channels.ContainsKey(key))
            {
                return _channels[key];
            }

            var model = _connection.Value.CreateModel();
            _channels.Add(key, model);
            return model;
        }

        /// <summary>
        /// Создает очередь, если она еще не существует.
        /// </summary>
        /// <param name="queueKey">Ключ/имя очереди.</param>
        /// <param name="channelKey"></param>
        /// <returns></returns>
        public string DeclareQueue(string queueKey, string channelKey)
        {
            string queueName;

            if (_queues.TryGetValue(queueKey, out queueName))
            {
                return queueName;
            }

            var channel = GetChannel(channelKey);
            channel.QueueDeclare(queueKey, Defaults.Queue.Durable, Defaults.Queue.Exclusive, Defaults.Queue.AutoDelete, null);
            channel.QueueBind(queueKey, _exchangeNames[ExchangeType.Direct], queueKey);

            queueName = _queues.GetOrAdd(queueKey, queueKey);

            return queueName;
        }
    }
}