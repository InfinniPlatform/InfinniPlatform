namespace InfinniPlatform.MessageQueue.RabbitMq.Outdated
{
	/// <summary>
	/// Менеджер сессий очереди сообщений.
	/// </summary>
	/// <returns>
	/// Реализует стратегию управления временем жизни сессии очереди сообщений.
	/// </returns>
	public interface IMessageQueueSessionManager
	{
		/// <summary>
		/// Открыть сессию.
		/// </summary>
		/// <returns>Сессия очереди сообщений.</returns>
		IMessageQueueSession OpenSession();

		/// <summary>
		/// Закрыть сессию.
		/// </summary>
		/// <param name="session">Сессия очереди сообщений.</param>
		void CloseSession(IMessageQueueSession session);
	}
}