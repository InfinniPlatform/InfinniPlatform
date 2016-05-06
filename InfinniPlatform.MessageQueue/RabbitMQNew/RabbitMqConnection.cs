using System;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class RabbitMqConnection
    {
        public RabbitMqConnection(RabbitMqConnectionSettings connectionSettings)
        {
            var connectionFactory = new ConnectionFactory
                                    {
                                        HostName = connectionSettings.HostName,
                                        Port = connectionSettings.Port
                                    };

            _connection = new Lazy<IConnection>(() => connectionFactory.CreateConnection());
        }

        private readonly Lazy<IConnection> _connection;

        public IConnection GetConnection()
        {
            return _connection.Value;
        }
    }
}