namespace InfinniPlatform.MessageQueue.RabbitMq.Outdated.Policies
{
	/// <summary>
	/// Решение о возможности повторного выполнения действия после неудачной попытки.
	/// </summary>
	public enum RetryDecision
	{
		/// <summary>
		/// Повторить попытку.
		/// </summary>
		Retry,

		/// <summary>
		/// Не повторять попытку, пробросить исключение.
		/// </summary>
		Rethrow,

		/// <summary>
		/// Не повторять попытку, игнорировать исключение.
		/// </summary>
		Ignore
	}
}