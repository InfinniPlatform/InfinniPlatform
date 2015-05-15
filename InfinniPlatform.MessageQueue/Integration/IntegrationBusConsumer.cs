using System;
using InfinniPlatform.Hosting;

namespace InfinniPlatform.MessageQueue.Integration
{
	/// <summary>
	/// Подписчик интеграционной шины.
	/// </summary>
	sealed class IntegrationBusConsumer : IQueueConsumer
	{
		public IntegrationBusConsumer(IIntegrationBusSubscriptionStorage subscriptionStorage)
		{
			if (subscriptionStorage == null)
			{
				throw new ArgumentNullException("subscriptionStorage");
			}

			_subscriptionStorage = subscriptionStorage;
		}


		private readonly IIntegrationBusSubscriptionStorage _subscriptionStorage;


		public void Handle(Message message)
		{
			// В данном месте осуществляется "redirect" из внутренней шины сообщений внешней информационной системе.
			// Предполагается, что внешняя информационная система имеет REST-сервис, способный принимать и обрабатывать
			// сообщения нашей интеграционной шины. Для популярных систем в дальнейшем могут быть разарботаны адаптеры
			// интеграции, которые будут размещаться на стороне внешней информационной системы, и предоставлять нужный
			// REST-сервис.

			var subscriptionAddress = _subscriptionStorage.GetSubscriptionAddress(message.ConsumerId);

			if (string.IsNullOrWhiteSpace(subscriptionAddress) == false)
			{
				var busMessage = new IntegrationBusMessage
									 {
										 ConsumerId = message.ConsumerId,
										 ExchangeName = message.Exchange,
										 RoutingKey = message.RoutingKey,
										 Properties = message.Properties,
										 Body = message.Body
									 };

				WebRequestHelper.Post(new Uri(subscriptionAddress), busMessage);
			}
		}
	}
}