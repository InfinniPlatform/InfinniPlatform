using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.UserInterface.ViewBuilders.Messaging
{
	/// <summary>
	/// Список подписок.
	/// </summary>
	sealed class SubscriptionList
	{
		private readonly ConcurrentDictionary<string, ConcurrentQueue<Subscription>> _subscriptions
			= new ConcurrentDictionary<string, ConcurrentQueue<Subscription>>();


		/// <summary>
		/// Добавляет подписку.
		/// </summary>
		/// <param name="messageType">Тип сообщения.</param>
		/// <param name="messageHandler">Обработчик сообщения.</param>
		public Subscription AddSubscription(string messageType, Action<dynamic> messageHandler)
		{
			var subscriptions = _subscriptions.GetOrAdd(messageType, t => new ConcurrentQueue<Subscription>());

			Subscription subscription;
			subscription = new Subscription(() => subscriptions.TryDequeue(out subscription), messageHandler);

			subscriptions.Enqueue(subscription);

			return subscription;
		}

		/// <summary>
		/// Возвращает список подписок.
		/// </summary>
		/// <param name="messageType">Тип сообщения.</param>
		public IEnumerable<Subscription> GetSubscriptions(string messageType)
		{
			ConcurrentQueue<Subscription> subscriptions;

			_subscriptions.TryGetValue(messageType, out subscriptions);

			return subscriptions ?? Enumerable.Empty<Subscription>();
		}
	}
}