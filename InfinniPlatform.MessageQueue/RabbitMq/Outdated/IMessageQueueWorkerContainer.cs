namespace InfinniPlatform.MessageQueue.RabbitMq.Outdated
{
	/// <summary>
	/// Контейнер рабочих потоков очередей сообщений.
	/// </summary>
	public interface IMessageQueueWorkerContainer
	{
		/// <summary>
		/// Зарегистрировать рабочий поток очереди сообщений.
		/// </summary>
		/// <param name="queueName">Наименование очереди сообщений.</param>
		/// <param name="queueWorker">Интерфейс для управления рабочим потоком очереди сообщений.</param>
		void RegisterWorker(string queueName, IMessageQueueWorker queueWorker);

		/// <summary>
		/// Отменить регистрацию рабочего потока очереди сообщений.
		/// </summary>
		/// <param name="queueName">Наименование очереди сообщений.</param>
		void UnregisterWorker(string queueName);
	}
}