using InfinniPlatform.Sdk.Queues.Outdated;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Util;

namespace InfinniPlatform.MessageQueue.RabbitMq.Outdated.Client
{
	/// <summary>
	/// Очередь сообщений RabbitMq.
	/// </summary>
	sealed class RabbitMqQueue : IMessageQueue
	{
		public RabbitMqQueue(IModel channel, SharedQueue<BasicDeliverEventArgs> queue, RabbitMqMessageConverter messageConverter)
		{
			_queue = queue;
			_channel = channel;
			_messageConverter = messageConverter;
		}


		private readonly IModel _channel;
		private readonly SharedQueue<BasicDeliverEventArgs> _queue;
		private readonly RabbitMqMessageConverter _messageConverter;


		/// <summary>
		/// Получить очередное сообщение.
		/// </summary>
		public Message Dequeue()
		{
			var queueEntry = _queue.Dequeue();

			return new Message
					   {
						   DeliveryTag = queueEntry.DeliveryTag,
						   ConsumerId = queueEntry.ConsumerTag,
						   Exchange = queueEntry.Exchange,
						   RoutingKey = queueEntry.RoutingKey,
						   Properties = _messageConverter.ConvertFrom(queueEntry.BasicProperties),
						   Body = queueEntry.Body
					   };
		}

		/// <summary>
		/// Подтвердить окончание обработки сообщения.
		/// </summary>
		/// <param name="message">Сообщение очереди.</param>
		public void Acknowledge(Message message)
		{
			_channel.BasicAck(message.DeliveryTag, false);
		}

		/// <summary>
		/// Отказаться от обработки сообщения.
		/// </summary>
		/// <param name="message">Сообщение очереди.</param>
		public void Reject(Message message)
		{
			_channel.BasicReject(message.DeliveryTag, true);
		}

		/// <summary>
		/// Освободить используемые ресурсы.
		/// </summary>
		public void Dispose()
		{
			_queue.Close();
		}
	}
}