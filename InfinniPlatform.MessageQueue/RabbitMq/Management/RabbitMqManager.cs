using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Settings;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMq.Management
{
    /// <summary>
    /// Менеджер соединения с RabbitMQ.
    /// </summary>
    internal sealed class RabbitMqManager : IDisposable
    {
        public RabbitMqManager(RabbitMqConnectionSettings rabbitMqConnectionSettings,
                               IAppEnvironment appEnvironment,
                               ILog log)
        {
            BroadcastExchangeName = $"{appEnvironment.Name}.{Defaults.Exchange.Type.Direct}";

            _connection = new Lazy<IConnection>(() => CreateConnection(rabbitMqConnectionSettings));
            _rabbitMqConnectionSettings = rabbitMqConnectionSettings;
            _log = log;
        }

        private readonly Lazy<IConnection> _connection;
        private readonly ILog _log;
        private readonly RabbitMqConnectionSettings _rabbitMqConnectionSettings;

        public string BroadcastExchangeName { get; }

        public void Dispose()
        {
            _connection.Value.Dispose();
        }

        private IConnection CreateConnection(RabbitMqConnectionSettings settings)
        {
            var connectionFactory = new ConnectionFactory
                                    {
                                        HostName = settings.HostName,
                                        Port = settings.Port,
                                        UserName = settings.UserName,
                                        Password = settings.Password,
                                        AutomaticRecoveryEnabled = Defaults.Connection.AutomaticRecoveryEnabled
                                    };

            var connection = connectionFactory.CreateConnection();

            connection.ConnectionShutdown += OnConnectionShutdown;

            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(BroadcastExchangeName, Defaults.Exchange.Type.Direct, Defaults.Exchange.Durable, Defaults.Exchange.AutoDelete, null);
            }

            return connection;
        }

        /// <summary>
        /// Возвращает экземпляр соединения с RabbitMq.
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
            try
            {
                var channel = _connection.Value.CreateModel();

                if (_rabbitMqConnectionSettings.PrefetchCount != 0)
                {
                    channel.BasicQos(0, _rabbitMqConnectionSettings.PrefetchCount, false);
                }

                return channel;
            }
            catch (Exception exception)
            {
                _log.Error(exception);
                return null;
            }
        }

        /// <summary>
        /// Создает канал.
        /// </summary>
        public IModel GetChannel(ushort prefetchCount)
        {
            try
            {
                var channel = _connection.Value.CreateModel();

                channel.BasicQos(0, prefetchCount, false);

                return channel;
            }
            catch (Exception exception)
            {
                _log.Error(exception);
                return null;
            }
        }

        /// <summary>
        /// Создает очередь для сообщений по ключу.
        /// </summary>
        /// <param name="queueKey">Ключ/имя очереди.</param>
        public void DeclareTaskQueue(string queueKey)
        {
            using (var channel = GetChannel())
            {
                channel.QueueDeclare(queueKey, Defaults.Queue.Durable, Defaults.Queue.Exclusive, Defaults.Queue.AutoDelete, null);
            }
        }

        /// <summary>
        /// Создает очередь для широковещательных сообщений.
        /// </summary>
        public string DeclareBroadcastQueue(string routingKey)
        {
            using (var channel = GetChannel())
            {
                var queueName = channel.QueueDeclare().QueueName;

                channel.QueueBind(queueName, BroadcastExchangeName, routingKey);

                return queueName;
            }
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs args)
        {
            _log.Error("RabbitMQ connection shutdown.", () => CreateLogContext(args));
        }

        private static Dictionary<string, object> CreateLogContext(ShutdownEventArgs args)
        {
            return new Dictionary<string, object>
                   {
                       { "Initiator", args.Initiator },
                       { "ReplyCode", args.ReplyCode },
                       { "ReplyText", args.ReplyText }
                   };
        }
    }
}