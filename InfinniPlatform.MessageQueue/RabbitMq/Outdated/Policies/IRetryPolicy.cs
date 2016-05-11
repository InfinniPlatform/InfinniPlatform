namespace InfinniPlatform.MessageQueue.RabbitMq.Outdated.Policies
{
	/// <summary>
	/// Политика для определения возможности повторного выполнения действия после неудачной попытки.
	/// </summary>
	public interface IRetryPolicy
	{
		/// <summary>
		/// Получить интерфейс для определения возможности повторного выполнения действия после неудачной попытки.
		/// </summary>
		IRetryScope NewScope();
	}
}