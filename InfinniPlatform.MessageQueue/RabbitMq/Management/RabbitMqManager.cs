using System;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Settings;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMq.Management
{
    /// <summary>
    /// Менеджер RabbitMQ, предоставляющий функции, доступные через RabbitMQ .NET-драйвер.
    /// </summary>
    internal sealed class RabbitMqManager
    {
        public RabbitMqManager(RabbitMqConnectionSettings settings,
                               IAppEnvironment appEnvironment,
                               ILog log)
        {
            _settings = settings;
            _log = log;
            BroadcastExchangeName = $"{appEnvironment.Name}.{Defaults.Exchange.Type.Fanout}";
            AppId = appEnvironment.Id;

            _connection = new Lazy<IConnection>(() =>
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
                                                    var channel = connection.CreateModel();
                                                    channel.ExchangeDeclare(BroadcastExchangeName, Defaults.Exchange.Type.Fanout, Defaults.Exchange.Durable, Defaults.Exchange.AutoDelete, null);
                                                    channel.Close();

                                                    return connection;
                                                });
        }

        private readonly Lazy<IConnection> _connection;
        private readonly ILog _log;
        private readonly RabbitMqConnectionSettings _settings;

        public string BroadcastExchangeName { get; }

        public string AppId { get; }

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
            try
            {
                var channel = _connection.Value.CreateModel();

                if (_settings.PrefetchSize != 0)
                {
                    channel.BasicQos(0, _settings.PrefetchSize, false);
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
    }
}