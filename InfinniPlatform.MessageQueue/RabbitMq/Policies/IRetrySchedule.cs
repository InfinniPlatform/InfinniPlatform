namespace InfinniPlatform.MessageQueue.RabbitMq.Policies
{
	/// <summary>
	/// Интерфейс для определения задержки перед попыткой повторного выполнения действия.
	/// </summary>
	public interface IRetrySchedule
	{
		/// <summary>
		/// Определить задержку перед попыткой повторного выполнения действия.
		/// </summary>
		/// <returns>Значение задержки в миллисекундах.</returns>
		int NextDelayMs();
	}
}