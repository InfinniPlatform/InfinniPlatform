using System;
using System.Collections.Generic;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class RabbitMqManager
    {
        public RabbitMqManager(RabbitMqConnectionSettings connectionSettings)
        {
            var connectionFactory = new ConnectionFactory
                                    {
                                        HostName = connectionSettings.HostName,
                                        Port = connectionSettings.Port
                                    };

            _connection = new Lazy<IConnection>(() => connectionFactory.CreateConnection());
        }

        private readonly Dictionary<string, IModel> _channels = new Dictionary<string, IModel>();

        private readonly Lazy<IConnection> _connection;
        private readonly Dictionary<string, string> _exchanges = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _queues = new Dictionary<string, string>();

        public IConnection GetConnection()
        {
            return _connection.Value;
        }

        public IModel GetModel(string key)
        {
            if (_channels.ContainsKey(key))
            {
                return _channels[key];
            }

            var model = _connection.Value.CreateModel();
            _channels.Add(key, model);
            return model;
        }

        public string GetExchange(string key)
        {
            if (_exchanges.ContainsKey(key))
            {
                return _exchanges[key];
            }

            GetModel(key).ExchangeDeclare(key, ExchangeType.Direct);
            _exchanges.Add(key, key);
            return key;
        }

        public string GetQueue(string key)
        {
            if (_queues.ContainsKey(key))
            {
                return _queues[key];
            }

            GetModel(key).QueueDeclare(key, false, false, false, null);
            _queues.Add(key, key);
            return key;
        }
    }
}