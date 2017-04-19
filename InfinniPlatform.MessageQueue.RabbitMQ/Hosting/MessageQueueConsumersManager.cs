using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Core.Logging;
using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;
using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.MessageQueue.RabbitMQ.Properties;
using InfinniPlatform.MessageQueue.RabbitMQ.Serialization;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMQ.Hosting
{
    /// <summary>
    /// Предоставляет метод регистрации получателей сообщений из очереди.
    /// </summary>
    [LoggerName("MessageQueue")]
    internal sealed class MessageQueueConsumersManager : IMessageQueueConsumersManager
    {
        /// <param name="messageConsumeEventHandler">Обработчик событий процесса обработки сообщения.</param>
        /// <param name="messageQueueThreadPool"></param>
        /// <param name="messageSerializer">Сериализатор сообщений.</param>
        /// <param name="manager">Менеджер соединения с RabbitMQ.</param>
        /// <param name="log">Лог.</param>
        /// <param name="performanceLog">Лог производительности.</param>
        public MessageQueueConsumersManager(IMessageConsumeEventHandler messageConsumeEventHandler,
                                            IMessageQueueThreadPool messageQueueThreadPool,
                                            IMessageSerializer messageSerializer,
                                            RabbitMqManager manager,
                                            ILog log,
                                            IPerformanceLog performanceLog)
        {
            _messageConsumeEventHandler = messageConsumeEventHandler;
            _messageQueueThreadPool = messageQueueThreadPool;
            _messageSerializer = messageSerializer;
            _manager = manager;
            _log = log;
            _performanceLog = performanceLog;
        }

        private readonly ILog _log;
        private readonly RabbitMqManager _manager;
        private readonly IMessageConsumeEventHandler _messageConsumeEventHandler;
        private readonly IMessageQueueThreadPool _messageQueueThreadPool;
        private readonly IMessageSerializer _messageSerializer;
        private readonly IPerformanceLog _performanceLog;

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
            eventingConsumer.Shutdown += (sender, args) => { _log.Error("Consumer shutdown.", () => CreateLogContext(consumer)); };

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
                                                      IMessage message = null;
                                                      var consumerType = consumer.GetType().Name;
                                                      Func<Dictionary<string, object>> logContext = () => CreateLogContext(consumerType, args);

                                                      try
                                                      {
                                                          message = _messageSerializer.BytesToMessage(args, consumer.MessageType);

                                                          _log.Debug(Resources.ConsumeStart, logContext);

                                                          await _messageConsumeEventHandler.OnBefore(message);

                                                          await consumer.Consume(message);

                                                          _log.Debug(Resources.ConsumeSuccess, logContext);

                                                          BasicAck(channel, args, logContext);
                                                      }
                                                      catch (Exception e)
                                                      {
                                                          error = e;

                                                          if (await _messageConsumeEventHandler.OnError(message, error))
                                                          {
                                                              if (await consumer.OnError(error))
                                                              {
                                                                  BasicAck(channel, args, logContext);
                                                              }
                                                          }

                                                          _log.Error(error, logContext);
                                                      }
                                                      finally
                                                      {
                                                          _performanceLog.Log($"Consume::{consumerType}", startDate, error);
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
                _log.Debug(Resources.AckStart, logContext);
                //TODO: При передаче параметра multiple = true, BasicAck бросает исключение "unknown delivery tag". Вероятно путаница с каналами.
                channel.BasicAck(args.DeliveryTag, false);
                _log.Debug(Resources.AckSuccess, logContext);
            }
            catch (Exception e)
            {
                _log.Error(e);
            }
        }

        private static Dictionary<string, object> CreateLogContext(IConsumer consumer)
        {
            return new Dictionary<string, object> { { "ConsumerType", consumer.GetType().Name } };
        }

        private static Dictionary<string, object> CreateLogContext(string consumerType, BasicDeliverEventArgs args)
        {
            return new Dictionary<string, object>
                   {
                       { "consumerType", consumerType },
                       { "deliveryTag", args?.DeliveryTag },
                       { "messageSize", args?.Body?.Length }
                   };
        }
    }
}