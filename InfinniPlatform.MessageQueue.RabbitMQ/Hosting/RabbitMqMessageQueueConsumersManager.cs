using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue.Management;
using InfinniPlatform.MessageQueue.Properties;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.Hosting
{
    /// <summary>
    /// Предоставляет метод регистрации получателей сообщений из очереди.
    /// </summary>
    [LoggerName("RabbitMqMessageQueue")]
    internal class RabbitMqMessageQueueConsumersManager : IMessageQueueConsumersManager
    {
        public RabbitMqMessageQueueConsumersManager(MessageQueueThreadPool messageQueueThreadPool,
                                                    IRabbitMqMessageSerializer messageSerializer,
                                                    RabbitMqManager manager,
                                                    ILog log,
                                                    IPerformanceLog performanceLog)
        {
            _messageQueueThreadPool = messageQueueThreadPool;
            _messageSerializer = messageSerializer;
            _manager = manager;
            _log = log;
            _performanceLog = performanceLog;
        }

        private readonly MessageQueueThreadPool _messageQueueThreadPool;
        private readonly IRabbitMqMessageSerializer _messageSerializer;
        private readonly RabbitMqManager _manager;
        private readonly ILog _log;
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
                                                      var consumerType = consumer.GetType().Name;

                                                      Func<Dictionary<string, object>> logContext = () => CreateLogContext(consumerType, args);

                                                      try
                                                      {
                                                          var message = _messageSerializer.BytesToMessage(args, consumer.MessageType);

                                                          _log.Debug(Resources.ConsumeStart, logContext);

                                                          await consumer.Consume(message);

                                                          _log.Debug(Resources.ConsumeSuccess, logContext);

                                                          BasicAck(channel, args, logContext);
                                                      }
                                                      catch (Exception e)
                                                      {
                                                          error = e;

                                                          if (await consumer.OnError(error))
                                                          {
                                                              BasicAck(channel, args, logContext);
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