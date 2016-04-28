namespace InfinniPlatform.MessageQueue.RabbitMq
{
	/// <summary>
	/// Фабрика сессий очереди сообщений.
	/// </summary>
	public interface IMessageQueueSessionFactory
	{
		/// <summary>
		/// Открыть сессию.
		/// </summary>
		IMessageQueueSession OpenSession();
	}
}