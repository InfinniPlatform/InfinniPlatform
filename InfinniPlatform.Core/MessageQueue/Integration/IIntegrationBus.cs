namespace InfinniPlatform.MessageQueue.Integration
{
	/// <summary>
	/// Сервис интеграционной шины.
	/// </summary>
	public interface IIntegrationBus
	{
		/// <summary>
		/// Подписаться на очередь сообщений.
		/// </summary>
		/// <param name="securityToken">Маркер безопасности.</param>
		/// <param name="subscription">Информация о подписке.</param>
		void Subscribe(string securityToken, IntegrationBusSubscription subscription);

		/// <summary>
		/// Удалить подписку на очередь сообщений.
		/// </summary>
		/// <param name="securityToken">Маркер безопасности.</param>
		/// <param name="subscription">Информация о подписке.</param>
		void Unsubscribe(string securityToken, IntegrationBusSubscription subscription);

		/// <summary>
		/// Опубликовать сообщение.
		/// </summary>
		/// <param name="securityToken">Маркер безопасности.</param>
		/// <param name="message">Сообщение.</param>
		void Publish(string securityToken, IntegrationBusMessage message);
	}
}