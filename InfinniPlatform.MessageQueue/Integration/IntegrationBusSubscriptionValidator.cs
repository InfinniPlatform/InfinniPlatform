using System;
using System.Text;

using InfinniPlatform.Core.Hosting;
using InfinniPlatform.MessageQueue.Properties;
using InfinniPlatform.Sdk.Queues.Integration;

namespace InfinniPlatform.MessageQueue.Integration
{
	/// <summary>
	/// Осуществляет проверку информации о подписке на очередь сообщений интеграционной шины.
	/// </summary>
	sealed class IntegrationBusSubscriptionValidator : IIntegrationBusSubscriptionValidator
	{
		/// <summary>
		/// Осуществить проверку информации о подписке и бросить исключение, если информация некорректна.
		/// </summary>
		/// <param name="subscription">Информация о подписке.</param>
		public void Validate(IntegrationBusSubscription subscription)
		{
			VerifySubscription(subscription);
		}

		/// <summary>
		/// Осуществить проверку информации о подписке, включая проверку доступности сервиса подписчика для отправки сообщений, и бросить исключение, если информация некорректна.
		/// </summary>
		/// <param name="subscription">Информация о подписке.</param>
		public void ValidateWithAddress(IntegrationBusSubscription subscription)
		{
			VerifySubscription(subscription);
			VerifySubscriptionAddress(subscription.Address);
		}


		private static void VerifySubscription(IntegrationBusSubscription subscription)
		{
			if (subscription == null)
			{
				throw new ArgumentNullException("subscription");
			}

			if (string.IsNullOrWhiteSpace(subscription.ConsumerId))
			{
				throw new ArgumentException(Resources.ConsumerIdCannotBeNullOrWhiteSpace, "subscription");
			}

			if (string.IsNullOrWhiteSpace(subscription.ExchangeType))
			{
				throw new ArgumentException(Resources.ExchangeTypeCannotBeNullOrWhiteSpace, "subscription");
			}

			if (string.IsNullOrWhiteSpace(subscription.ExchangeName))
			{
				throw new ArgumentException(Resources.ExchangeNameCannotBeNullOrWhiteSpace, "subscription");
			}

			if (string.IsNullOrWhiteSpace(subscription.QueueName))
			{
				throw new ArgumentException(Resources.QueueNameCannotBeNullOrWhiteSpace, "subscription");
			}

			if (string.IsNullOrWhiteSpace(subscription.Address))
			{
				throw new ArgumentException(Resources.AddressCannotBeNullOrWhiteSpace, "subscription");
			}

			Uri address;

			if (Uri.TryCreate(subscription.Address, UriKind.Absolute, out address)
				&& (address.Scheme == Uri.UriSchemeHttp || address.Scheme == Uri.UriSchemeHttps))
			{
				throw new ArgumentException(Resources.AddressShouldBeAbsoluteUriToHttpOrHttpsService, "subscription");
			}
		}

		private static void VerifySubscriptionAddress(string subscriptionAddress)
		{
			var testMessage = new IntegrationBusMessage { ConsumerId = "Test", ExchangeName = "Test", RoutingKey = "Test", Body = Encoding.UTF8.GetBytes("Test") };

			try
			{
				// На адрес, указанный в подписке, отправляется тестовый пакет. Если отправка прошла успешно, считается, что адрес прошел проверку.
				// Это сделано для того, чтобы у подписчиков не было возможности передать адрес на несуществующий сервис. Может получиться так, что
				// внешняя система подпишется, передав недействительный адрес сервиса (возможно, даже ненамеренно), и будет обращаться в службу
				// поддержки с вопросом, почему им не приходят сообщения, несмотря на то, что они подписались. Эта ситуация повлечет трудозатраты
				// со стороны разработчиков, но хуже всего, что эти сообщения будут накапливаться в очереди, увеличивая оперативную память на
				// сервере шины сообщений. Как раз чтобы не допустить подобную ситуацию, в момет подписки сразу же проверяется доступность
				// сервиса подписчика. Естественно, что и после успешной подписки этот сервис может по каким-либо причинам стать недоступным,
				// но это уже, скорей, административная задача.

				WebRequestHelper.Post(new Uri(subscriptionAddress), testMessage);
			}
			catch (Exception error)
			{
				throw new ArgumentException(string.Format(Resources.SubscriptionServiceIsNotAvailable, subscriptionAddress), "subscriptionAddress", error);
			}
		}
	}
}