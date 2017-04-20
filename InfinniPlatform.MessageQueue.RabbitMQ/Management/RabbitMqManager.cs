using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Logging;
using InfinniPlatform.Settings;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMq.Management
{
    /// <summary>
    /// Менеджер соединения с RabbitMQ.
    /// </summary>
    internal sealed class RabbitMqManager : IDisposable
    {
        public RabbitMqManager(RabbitMqConnectionSettings rabbitMqConnectionSettings,
                               AppOptions appOptions,
                               ILog log)
        {
            BroadcastExchangeName = $"{appOptions.AppName}.{Defaults.Exchange.Type.Direct}";

            _appOptions = appOptions;
            _rabbitMqConnectionSettings = rabbitMqConnectionSettings;
            _log = log;

            var connection = CreateConnection(rabbitMqConnectionSettings);

            if (connection == null)
            {
                StartRabbitMqListener(rabbitMqConnectionSettings);
            }
            else
            {
                Connection = connection;
            }
        }

        private readonly AppOptions _appOptions;
        private readonly ILog _log;
        private readonly RabbitMqConnectionSettings _rabbitMqConnectionSettings;

        public string BroadcastExchangeName { get; }

        public IConnection Connection { get; private set; }

        public void Dispose()
        {
            Connection.Dispose();
        }

        /// <summary>
        /// Событие, вызваемое при восстановлении соединения с сервером RabbitMq.
        /// </summary>
        public event ReconnectEventHandler OnReconnect;

        private void StartRabbitMqListener(RabbitMqConnectionSettings settings)
        {
            Task.Run(() =>
                     {
                         IConnection connection = null;

                         while (connection == null)
                         {
                             connection = CreateConnection(settings);
                             Task.Delay(TimeSpan.FromSeconds(settings.ReconnectTimeout));
                         }

                         Connection = connection;
                         OnReconnect?.Invoke(this, new RabbitMqReconnectEventArgs());
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
                var channel = Connection.CreateModel();

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
        /// Создает очередь для сообщений по имени.
        /// </summary>
        /// <param name="queueName">Имя очереди.</param>
        public string DeclareTaskQueue(string queueName)
        {
            using (var channel = GetChannel())
            {
                var declaredQueueName = GetTaskKey(queueName);

                channel?.QueueDeclare(declaredQueueName, Defaults.Queue.Durable, Defaults.Queue.Exclusive, Defaults.Queue.AutoDelete);

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

                channel?.QueueDeclare(declaredQueueName, Defaults.Queue.Durable, Defaults.Queue.Exclusive, Defaults.Queue.AutoDelete);
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

        private IConnection CreateConnection(RabbitMqConnectionSettings settings)
        {
            try
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
            catch (Exception e)
            {
                _log.Error(e);
                return null;
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


        internal delegate void ReconnectEventHandler(object sender, RabbitMqReconnectEventArgs e);
    }


    internal class RabbitMqReconnectEventArgs
    {
    }
}