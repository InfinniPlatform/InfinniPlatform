using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue.Properties;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.Management
{
    /// <summary>
    /// Менеджер соединения с RabbitMQ.
    /// </summary>
    [LoggerName(nameof(RabbitMqManager))]
    public class RabbitMqManager : IDisposable
    {
        public delegate void ReconnectEventHandler(object sender, RabbitMqReconnectEventArgs e);

        private readonly AppOptions _appOptions;
        private readonly ILogger _logger;


        private readonly RabbitMqMessageQueueOptions _options;

        public RabbitMqManager(RabbitMqMessageQueueOptions options,
                               AppOptions appOptions,
                               ILogger<RabbitMqManager> logger)
        {
            BroadcastExchangeName = $"{appOptions.AppName}.{RabbitMqDefaults.Exchange.Type.Direct}";

            _appOptions = appOptions;
            _options = options;
            _logger = logger;

            var connection = CreateConnection(options);

            if (connection == null)
            {
                StartRabbitMqListener(options);
            }
            else
            {
                Connection = connection;
            }
        }


        public string BroadcastExchangeName { get; }

        public IConnection Connection { get; private set; }

        public void Dispose()
        {
            Connection?.Dispose();
        }

        /// <summary>
        /// Событие, вызываемое при восстановлении соединения с сервером RabbitMq.
        /// </summary>
        public event ReconnectEventHandler OnReconnect;

        private void StartRabbitMqListener(RabbitMqMessageQueueOptions settings)
        {
            Task.Run(async () =>
            {
                IConnection connection = null;
                var reconnectRetries = settings.MaxReconnectRetries;

                while (connection == null && reconnectRetries > 0)
                {
                    connection = CreateConnection(settings);

                    if (connection == null)
                    {
                        reconnectRetries--;
                        await Task.Delay(TimeSpan.FromSeconds(settings.ReconnectTimeout));
                    }
                }

                if (connection == null)
                {
                    _logger.LogCritical(string.Format(Resources.ReconnectRetriesExceeded, settings.HostName, settings.Port));
                }
                else
                {
                    Connection = connection;
                    OnReconnect?.Invoke(this, new RabbitMqReconnectEventArgs());
                }
            });
        }

        /// <summary>
        /// Создает канал.
        /// </summary>
        public IModel GetChannel()
        {
            try
            {
                var channel = Connection.CreateModel();

                if (_options.PrefetchCount != 0)
                {
                    channel.BasicQos(0, _options.PrefetchCount, false);
                }

                return channel;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception);
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
                var channel = Connection.CreateModel();

                channel.BasicQos(0, prefetchCount, false);

                return channel;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception);
                return null;
            }
        }

        /// <summary>
        /// Создает очередь для сообщений по имени.
        /// </summary>
        /// <param name="queueName">Имя очереди.</param>
        public string DeclareTaskQueue(string queueName)
        {
            using (var channel = GetChannel())
            {
                var declaredQueueName = GetTaskKey(queueName);

                channel?.QueueDeclare(declaredQueueName, RabbitMqDefaults.Queue.Durable, RabbitMqDefaults.Queue.Exclusive, RabbitMqDefaults.Queue.AutoDelete);

                return declaredQueueName;
            }
        }

        /// <summary>
        /// Создает очередь для широковещательных сообщений по ключу.
        /// </summary>
        /// <param name="routingKey">Ключ.</param>
        public string DeclareBroadcastQueue(string routingKey)
        {
            using (var channel = GetChannel())
            {
                var declaredQueueName = GetBroadcastKey(routingKey);

                channel?.QueueDeclare(declaredQueueName, RabbitMqDefaults.Queue.Durable, RabbitMqDefaults.Queue.Exclusive, RabbitMqDefaults.Queue.AutoDelete);
                channel?.QueueBind(declaredQueueName, BroadcastExchangeName, routingKey);

                return declaredQueueName;
            }
        }

        /// <summary>
        /// Создает ключ из ключа.
        /// </summary>
        /// <param name="key">Ключ.</param>
        public string GetTaskKey(string key)
        {
            return $"{_appOptions.AppName}.{key}";
        }

        /// <summary>
        /// Создает ключ из ключа.
        /// </summary>
        /// <param name="key">Ключ.</param>
        public string GetBroadcastKey(string key)
        {
            var broadcastKey = $"{_appOptions.AppName}.{key}";

            if (!string.IsNullOrEmpty(_appOptions.AppInstance))
            {
                broadcastKey += $".{_appOptions.AppInstance}";
            }

            return broadcastKey;
        }

        private IConnection CreateConnection(RabbitMqMessageQueueOptions settings)
        {
            try
            {
                var connectionFactory = new ConnectionFactory
                {
                    HostName = settings.HostName,
                    Port = settings.Port,
                    UserName = settings.UserName,
                    Password = settings.Password
                };

                var connection = connectionFactory.CreateConnection();

                connection.ConnectionShutdown += OnConnectionShutdown;

                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(BroadcastExchangeName, RabbitMqDefaults.Exchange.Type.Direct, RabbitMqDefaults.Exchange.Durable);
                }

                return connection;
            }
            catch (Exception e)
            {
                _logger.LogError(e);
                return null;
            }
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs args)
        {
            if (args.Initiator == ShutdownInitiator.Application && args.ReplyCode == 200)
            {
                _logger.LogInformation(args.ReplyText, () => CreateLogContext(args));
            }
            else
            {
                _logger.LogError(args.ReplyText, () => CreateLogContext(args));
            }
        }

        private static Dictionary<string, object> CreateLogContext(ShutdownEventArgs args)
        {
            return new Dictionary<string, object>
            {
                {"initiator", args.Initiator},
                {"replyCode", args.ReplyCode}
            };
        }
    }


    public class RabbitMqReconnectEventArgs
    {
    }
}