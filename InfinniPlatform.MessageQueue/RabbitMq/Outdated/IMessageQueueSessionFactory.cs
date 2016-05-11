namespace InfinniPlatform.MessageQueue.RabbitMq.Outdated
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