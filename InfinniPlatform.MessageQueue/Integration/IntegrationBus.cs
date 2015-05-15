using System;
using System.Collections.Generic;

using InfinniPlatform.Factories;
using InfinniPlatform.MessageQueue.Properties;
using InfinniPlatform.RabbitMq;

namespace InfinniPlatform.MessageQueue.Integration
{
	/// <summary>
	/// Сервис интеграционной шины.
	/// </summary>
	sealed class IntegrationBus : IIntegrationBus
	{
		public IntegrationBus(IMessageQueueFactory queueFactory, IIntegrationBusStorageFactory subscriptionStorageFactory, IIntegrationBusSecurityTokenValidator securityTokenValidator, IIntegrationBusSubscriptionValidator subscriptionValidator)
		{
			if (queueFactory == null)
			{
				throw new ArgumentNullException("queueFactory");
			}

			if (subscriptionStorageFactory == null)
			{
				throw new ArgumentNullException("subscriptionStorageFactory");
			}

			if (securityTokenValidator == null)
			{
				throw new ArgumentNullException("securityTokenValidator");
			}

			if (subscriptionValidator == null)
			{
				throw new ArgumentNullException("subscriptionValidator");
			}

			_queueManager = queueFactory.CreateMessageQueueManager();
			_queueListener = queueFactory.CreateMessageQueueListener();
			_queuePublisher = queueFactory.CreateMessageQueuePublisher();

			_subscriptionStorage = subscriptionStorageFactory.CreateSubscriptionStorage();
			_queueConsumer = new IntegrationBusConsumer(_subscriptionStorage);

			_securityTokenValidator = securityTokenValidator;
			_subscriptionValidator = subscriptionValidator;

			_subscribers.Add("fanout", SubscribeFanout);
			_subscribers.Add("direct", SubscribeDirect);
			_subscribers.Add("topic", SubscribeTopic);
			_subscribers.Add("headers", SubscribeHeaders);

			_unsubscribers.Add("fanout", UnsubscribeFanout);
			_unsubscribers.Add("direct", UnsubscribeDirect);
			_unsubscribers.Add("topic", UnsubscribeTopic);
			_unsubscribers.Add("headers", UnsubscribeHeaders);
		}


		private readonly IMessageQueueManager _queueManager;
		private readonly IMessageQueueListener _queueListener;
		private readonly IMessageQueuePublisher _queuePublisher;
		private readonly IntegrationBusConsumer _queueConsumer;

		private readonly IIntegrationBusSecurityTokenValidator _securityTokenValidator;
		private readonly IIntegrationBusSubscriptionValidator _subscriptionValidator;
		private readonly IIntegrationBusSubscriptionStorage _subscriptionStorage;


		/// <summary>
		/// Подписаться на очередь сообщений.
		/// </summary>
		/// <param name="securityToken">Маркер безопасности.</param>
		/// <param name="subscription">Информация о подписке.</param>
		public void Subscribe(string securityToken, IntegrationBusSubscription subscription)
		{
			// Проверка информации о подписке

			_securityTokenValidator.Validate(securityToken);
			_subscriptionValidator.ValidateWithAddress(subscription);

			// Получение метода подписки на очередь сообщений

			Action<IntegrationBusSubscription> subscriber;

			var exchangeType = subscription.ExchangeType.Trim().ToLower();

			if (_subscribers.TryGetValue(exchangeType, out subscriber) == false)
			{
				throw new NotSupportedException(string.Format(Resources.SpecifiedExchangeTypeIsNotSupported, subscription.ExchangeType));
			}

			// Осуществление подписки на очередь сообщений
			subscriber(subscription);

			// Сохранение информации о подписке в хранилище
			_subscriptionStorage.AddSubscription(subscription);

			// Запуск прослушивания очереди сообщений
			_queueListener.StartListen(subscription.QueueName);
		}

		private readonly Dictionary<string, Action<IntegrationBusSubscription>> _subscribers
			= new Dictionary<string, Action<IntegrationBusSubscription>>();

		private void SubscribeFanout(IntegrationBusSubscription subscription)
		{
			_queueManager
				.GetExchangeFanout(subscription.ExchangeName)
				.Subscribe(subscription.QueueName, () => _queueConsumer, QueueConfig(subscription));
		}

		private void SubscribeDirect(IntegrationBusSubscription subscription)
		{
			_queueManager
				.GetExchangeDirect(subscription.ExchangeName)
				.Subscribe(subscription.QueueName, () => _queueConsumer, subscription.RoutingKey, QueueConfig(subscription));
		}

		private void SubscribeTopic(IntegrationBusSubscription subscription)
		{
			_queueManager
				.GetExchangeTopic(subscription.ExchangeName)
				.Subscribe(subscription.QueueName, () => _queueConsumer, subscription.RoutingKey, QueueConfig(subscription));
		}

		private void SubscribeHeaders(IntegrationBusSubscription subscription)
		{
			_queueManager
				.GetExchangeHeaders(subscription.ExchangeName)
				.Subscribe(subscription.QueueName, () => _queueConsumer, subscription.RoutingHeaders, QueueConfig(subscription));
		}

		private static Action<IQueueConfig> QueueConfig(IntegrationBusSubscription subscription)
		{
			return config => config.ConsumerId(subscription.ConsumerId)
								   .AcknowledgePolicy(new OnlyOnSuccessHandlingAcknowledgePolicy())
								   .WorkerThreadCount(1)
								   .Durable();
		}


		/// <summary>
		/// Удалить подписку на очередь сообщений.
		/// </summary>
		/// <param name="securityToken">Маркер безопасности.</param>
		/// <param name="subscription">Информация о подписке.</param>
		public void Unsubscribe(string securityToken, IntegrationBusSubscription subscription)
		{
			// Проверка информации о подписке

			_securityTokenValidator.Validate(securityToken);
			_subscriptionValidator.Validate(subscription);

			// Получение метода удаления подписки на очередь сообщений

			Action<IntegrationBusSubscription> unsubscriber;

			var exchangeType = subscription.ExchangeType.Trim().ToLower();

			if (_unsubscribers.TryGetValue(exchangeType, out unsubscriber) == false)
			{
				throw new NotSupportedException(string.Format(Resources.SpecifiedExchangeTypeIsNotSupported, subscription.ExchangeType));
			}

			// Остановка прослушивания очереди сообщений
			_queueListener.StopListen(subscription.QueueName);

			// Удаление информации о подписке в хранилище
			_subscriptionStorage.RemoveSubscription(subscription.ConsumerId);

			// Удаление подписки на очередь сообщений
			unsubscriber(subscription);
		}

		private readonly Dictionary<string, Action<IntegrationBusSubscription>> _unsubscribers
			= new Dictionary<string, Action<IntegrationBusSubscription>>();

		private void UnsubscribeFanout(IntegrationBusSubscription subscription)
		{
			_queueManager
				.GetExchangeFanout(subscription.ExchangeName)
				.Unsubscribe(subscription.QueueName);
		}

		private void UnsubscribeDirect(IntegrationBusSubscription subscription)
		{
			_queueManager
				.GetExchangeDirect(subscription.ExchangeName)
				.Unsubscribe(subscription.QueueName);
		}
		private void UnsubscribeTopic(IntegrationBusSubscription subscription)
		{
			_queueManager
				.GetExchangeTopic(subscription.ExchangeName)
				.Unsubscribe(subscription.QueueName);
		}
		private void UnsubscribeHeaders(IntegrationBusSubscription subscription)
		{
			_queueManager
				.GetExchangeHeaders(subscription.ExchangeName)
				.Unsubscribe(subscription.QueueName);
		}


		/// <summary>
		/// Опубликовать сообщение.
		/// </summary>
		/// <param name="securityToken">Маркер безопасности.</param>
		/// <param name="message">Сообщение.</param>
		public void Publish(string securityToken, IntegrationBusMessage message)
		{
			_securityTokenValidator.Validate(securityToken);

			if (message == null)
			{
				throw new ArgumentNullException("message");
			}

			if (string.IsNullOrWhiteSpace(message.ExchangeName))
			{
				throw new ArgumentException(Resources.ExchangeNameCannotBeNullOrWhiteSpace, "message");
			}

			_queuePublisher.Publish(message.ExchangeName, message.RoutingKey, message.Properties, message.Body);
		}
	}
}