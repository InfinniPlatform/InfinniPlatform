namespace InfinniPlatform.RabbitMq.Policies
{
	/// <summary>
	/// Политика для определения задержки между попытками повторного выполнения действия.
	/// </summary>
	public interface IRetrySchedulePolicy
	{
		/// <summary>
		/// Получить интерфейс для определения задержки перед попыткой повторного выполнения действия.
		/// </summary>
		IRetrySchedule NewSchedule();
	}
}