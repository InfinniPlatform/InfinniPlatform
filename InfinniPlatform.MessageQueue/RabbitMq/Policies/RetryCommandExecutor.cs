using System;
using System.Threading;

namespace InfinniPlatform.MessageQueue.RabbitMq.Policies
{
	/// <summary>
	/// Исполнитель команд с возможностью повторного выполнения команды в случае неудачной попытки.
	/// </summary>
	public sealed class RetryCommandExecutor : ICommandExecutor
	{
		public RetryCommandExecutor(IRetryPolicy retryPolicy, IRetrySchedulePolicy retrySchedulePolicy)
		{
			if (retryPolicy == null)
			{
				throw new ArgumentNullException("retryPolicy");
			}

			if (retrySchedulePolicy == null)
			{
				throw new ArgumentNullException("retrySchedulePolicy");
			}

			_retryPolicy = retryPolicy;
			_retrySchedulePolicy = retrySchedulePolicy;
		}


		private readonly IRetryPolicy _retryPolicy;
		private readonly IRetrySchedulePolicy _retrySchedulePolicy;


		/// <summary>
		/// Выполнить команду.
		/// </summary>
		/// <param name="command">Команда.</param>
		public void Execute(Action command)
		{
			try
			{
				command();
			}
			catch (Exception exception)
			{
				var retryScope = _retryPolicy.NewScope();
				var retrySchedule = _retrySchedulePolicy.NewSchedule();

				RetryExecute(command, exception, retryScope, retrySchedule);
			}
		}

		private static void RetryExecute(Action command, Exception error, IRetryScope retryScope, IRetrySchedule retrySchedule)
		{
			var retryDecision = retryScope.OnError(error);

			switch (retryDecision)
			{
				case RetryDecision.Retry:
					break;
				case RetryDecision.Ignore:
					return;
				default:
					throw error;
			}

			Thread.Sleep(retrySchedule.NextDelayMs());

			try
			{
				command();
			}
			catch (Exception exception)
			{
				RetryExecute(command, exception, retryScope, retrySchedule);
			}
		}
	}
}