using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.Properties;

using Microsoft.Extensions.Logging;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.Management
{
    /// <summary>
    /// Менеджер соединения с RabbitMQ.
    /// </summary>
    internal class RabbitMqManager : IDisposable
    {
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


        private readonly RabbitMqMessageQueueOptions _options;
        private readonly AppOptions _appOptions;
        private readonly ILogger _logger;


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
            Task.Run(() =>
                     {
                         IConnection connection = null;
                         var reconnectRetries = settings.MaxReconnectRetries;

                         while (connection == null && reconnectRetries > 0)
                         {
                             connection = CreateConnection(settings);

                             if (connection == null)
                             {
                                 reconnectRetries--;
                                 Task.Delay(TimeSpan.FromSeconds(settings.ReconnectTimeout));
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
            return $"{_appOptions.AppName}.{key}.{_appOptions.AppInstance}";
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
            _logger.LogError("RabbitMQ connection shutdown.", () => CreateLogContext(args));
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


        internal delegate void ReconnectEventHandler(object sender, RabbitMqReconnectEventArgs e);
    }


    internal class RabbitMqReconnectEventArgs
    {
    }
}