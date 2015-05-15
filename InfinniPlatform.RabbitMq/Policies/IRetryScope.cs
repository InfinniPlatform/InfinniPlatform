using System;

namespace InfinniPlatform.RabbitMq.Policies
{
	/// <summary>
	/// Интерфейс для определения возможности повторного выполнения действия после неудачной попытки.
	/// </summary>
	public interface IRetryScope
	{
		/// <summary>
		/// Действие было выполнено неуспешно.
		/// </summary>
		/// <param name="error">Ошибка при выполнении действия.</param>
		/// <returns>Решение о возможности повторного выполнения действия.</returns>
		RetryDecision OnError(Exception error);
	}
}