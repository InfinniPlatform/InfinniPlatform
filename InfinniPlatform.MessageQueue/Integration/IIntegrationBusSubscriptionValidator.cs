using InfinniPlatform.Sdk.Queues.Outdated.Integration;

namespace InfinniPlatform.MessageQueue.Integration
{
	/// <summary>
	/// Осуществляет проверку информации о подписке на очередь сообщений интеграционной шины.
	/// </summary>
	interface IIntegrationBusSubscriptionValidator
	{
		/// <summary>
		/// Осуществить проверку информации о подписке и бросить исключение, если информация некорректна.
		/// </summary>
		/// <param name="subscription">Информация о подписке.</param>
		void Validate(IntegrationBusSubscription subscription);

		/// <summary>
		/// Осуществить проверку информации о подписке, включая проверку доступности сервиса подписчика для отправки сообщений, и бросить исключение, если информация некорректна.
		/// </summary>
		/// <param name="subscription">Информация о подписке.</param>
		void ValidateWithAddress(IntegrationBusSubscription subscription);
	}
}