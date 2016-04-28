namespace InfinniPlatform.MessageQueue.RabbitMq
{
	/// <summary>
	/// Менеджер для управления рабочим потоком очереди сообщений.
	/// </summary>
	public interface IMessageQueueWorker
	{
		/// <summary>
		/// Запустить рабочий поток.
		/// </summary>
		void Start();

		/// <summary>
		/// Остановить рабочий поток.
		/// </summary>
		void Stop();
	}
}