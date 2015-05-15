using System;

namespace InfinniPlatform.RabbitMq.Policies
{
	/// <summary>
	/// Провайдер для определения возможности повторного выполнения действия после неудачной попытки.
	/// </summary>
	public interface IRetryDecisionProvider
	{
		/// <summary>
		/// Получить решение о возможности повторного выполнения действия после неудачной попытки.
		/// </summary>
		/// <param name="error">Ошибка при выполнении действия.</param>
		RetryDecision GetRetryDecision(Exception error);
	}
}