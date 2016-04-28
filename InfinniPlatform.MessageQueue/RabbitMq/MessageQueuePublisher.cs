using System;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
	/// <summary>
	/// Сервис для публикации сообщений.
	/// </summary>
	public sealed class MessageQueuePublisher : IMessageQueuePublisher
	{
		public MessageQueuePublisher(IMessageQueueCommandExecutor queueCommandExecutor)
		{
			if (queueCommandExecutor == null)
			{
				throw new ArgumentNullException("queueCommandExecutor");
			}

			_queueCommandExecutor = queueCommandExecutor;
		}


		private readonly IMessageQueueCommandExecutor _queueCommandExecutor;


		public void Publish(string exchange, string routingKey, MessageProperties properties, byte[] body)
		{
			if (string.IsNullOrWhiteSpace(exchange))
			{
				throw new ArgumentNullException("exchange");
			}

			_queueCommandExecutor.Execute(session => session.Publish(exchange, routingKey, properties, body));
		}
	}
}