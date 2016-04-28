using System;
using System.IO;

using InfinniPlatform.MessageQueue.RabbitMq.Policies;

using RabbitMQ.Client.Exceptions;

namespace InfinniPlatform.MessageQueue.RabbitMq.Client
{
	/// <summary>
	/// Провайдер для определения возможности повторного выполнения действия после неудачной попытки обращения к RabbitMq.
	/// </summary>
	public sealed class RabbitMqRetryDecisionProvider : IRetryDecisionProvider
	{
		public RetryDecision GetRetryDecision(Exception error)
		{
			// Если нет ошибки
			if (error == null)
			{
				return RetryDecision.Ignore;
			}

			// Все сетевые сбои
			if (error is IOException ||
				error is OperationInterruptedException ||
				error is ChannelAllocationException ||
				error is ConnectFailureException ||
				error is UnexpectedMethodException)
			{
				return RetryDecision.Retry;
			}

			return RetryDecision.Rethrow;
		}
	}
}