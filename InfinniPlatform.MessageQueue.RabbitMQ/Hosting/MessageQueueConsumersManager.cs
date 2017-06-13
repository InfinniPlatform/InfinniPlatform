using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue.Management;
using InfinniPlatform.MessageQueue.Properties;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.Hosting
{
    /// <summary>
    /// Предоставляет метод регистрации получателей сообщений из очереди.
    /// </summary>
    [LoggerName(nameof(MessageQueueConsumersManager))]
    public class MessageQueueConsumersManager : IMessageQueueConsumersManager
    {
        private readonly ILogger _logger;
        private readonly RabbitMqManager _manager;

        private readonly MessageQueueThreadPool _messageQueueThreadPool;
        private readonly IMessageSerializer _messageSerializer;
        private readonly IPerformanceLogger _perfLogger;

        public MessageQueueConsumersManager(MessageQueueThreadPool messageQueueThreadPool,
                                            IMessageSerializer messageSerializer,
                                            RabbitMqManager manager,
                                            ILogger<MessageQueueConsumersManager> logger,
                                            IPerformanceLogger<MessageQueueConsumersManager> perfLogger)
        {
            _messageQueueThreadPool = messageQueueThreadPool;
            _messageSerializer = messageSerializer;
            _manager = manager;
            _logger = logger;
            _perfLogger = perfLogger;
        }

        /// <summary>
        /// Регистрирует обработчик.
        /// </summary>
        /// <param name="queueName">Имя очереди.</param>
        /// <param name="consumer">Экземпляр получателя.</param>
        public void RegisterConsumer(string queueName, IConsumer consumer)
        {
            if (queueName == null)
            {
                throw new ArgumentException(Resources.UnableToGetQueueName);
            }

            var channel = _manager.GetChannel();

            var eventingConsumer = new EventingBasicConsumer(channel);

            eventingConsumer.Received += async (o, args) => await OnRecieved(consumer, args, channel);
            eventingConsumer.Shutdown += (sender, args) =>
            {
                if (args.Initiator == ShutdownInitiator.Application && args.ReplyCode == 200)
                {
                    _logger.LogInformation(args.ReplyText, () => CreateLogContext(consumer));
                }
                else
                {
                    _logger.LogError(args.ReplyText, () => CreateLogContext(consumer));
                }
            };

            channel?.BasicConsume(queueName, false, eventingConsumer);
        }

        /// <summary>
        /// Обработчик события получения сообщения из очереди.
        /// </summary>
        /// <param name="consumer">Экземпляр получателя.</param>
        /// <param name="args">Свойства сообщения.</param>
        /// <param name="channel">Канал RabbitMQ.</param>
        private async Task OnRecieved(IConsumer consumer, BasicDeliverEventArgs args, IModel channel)
        {
            var startDate = DateTime.Now;
            Exception error = null;

            await _messageQueueThreadPool.Enqueue(async () =>
            {
                var consumerType = consumer.GetType().Name;

                Func<Dictionary<string, object>> logContext = () => CreateLogContext(consumerType, args);

                try
                {
                    var message = _messageSerializer.BytesToMessage(args, consumer.MessageType);

                    _logger.LogDebug(Resources.ConsumeStart, logContext);

                    await consumer.Consume(message);

                    _logger.LogDebug(Resources.ConsumeSuccess, logContext);

                    BasicAck(channel, args, logContext);
                }
                catch (Exception e)
                {
                    error = e;

                    if (await consumer.OnError(error))
                    {
                        BasicAck(channel, args, logContext);
                    }

                    _logger.LogError(error, logContext);
                }
                finally
                {
                    _perfLogger.Log($"Consume::{consumerType}", startDate, error);
                }
            });
        }

        /// <summary>
        /// Обертка для подтверждения обработки сообщения.
        /// </summary>
        /// <param name="channel">Канал.</param>
        /// <param name="args">Свойства сообщения.</param>
        /// <param name="logContext">Контекст логирования.</param>
        private void BasicAck(IModel channel, BasicDeliverEventArgs args, Func<Dictionary<string, object>> logContext)
        {
            try
            {
                _logger.LogDebug(Resources.AckStart, logContext);
                //TODO: При передаче параметра multiple = true, BasicAck бросает исключение "unknown delivery tag". Вероятно путаница с каналами.
                channel.BasicAck(args.DeliveryTag, false);
                _logger.LogDebug(Resources.AckSuccess, logContext);
            }
            catch (Exception e)
            {
                _logger.LogError(e);
            }
        }

        private static Dictionary<string, object> CreateLogContext(IConsumer consumer)
        {
            return new Dictionary<string, object> {{"consumerType", consumer.GetType().Name}};
        }

        private static Dictionary<string, object> CreateLogContext(string consumerType, BasicDeliverEventArgs args)
        {
            return new Dictionary<string, object>
            {
                {"consumerType", consumerType},
                {"deliveryTag", args?.DeliveryTag},
                {"messageSize", args?.Body?.Length}
            };
        }
    }
}