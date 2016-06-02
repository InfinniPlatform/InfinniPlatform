using System;
using System.Threading;

using InfinniPlatform.MessageQueue.Properties;
using InfinniPlatform.Sdk.Settings;

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace InfinniPlatform.MessageQueue.RabbitMq.Connection
{
    /// <summary>
    /// Менеджер RabbitMQ, инкапсулирующий функции, доступные через RabbitMQ .NET-драйвер.
    /// </summary>
    internal sealed class RabbitMqManager
    {
        public RabbitMqManager(RabbitMqConnectionSettings connectionSettings,
                               IAppEnvironment appEnvironment)
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
        }

        private readonly Lazy<IConnection> _connection;

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
            var aggregateException = new AggregateException();

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
                    aggregateException = new AggregateException(e);
                    Thread.Sleep(1000);
                }
            }
            if (aggregateException.InnerExceptions.Count == 0)
            {
                throw new AggregateException(new InvalidOperationException(Resources.UnableToCreateRabbitMQChannel));
            }

            throw aggregateException;
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
    }
}