using System;
using System.Threading.Tasks;

using Microsoft.Owin;

using InfinniPlatform.Api.Profiling;
using InfinniPlatform.Owin.Properties;

namespace InfinniPlatform.Owin.Middleware
{
	/// <summary>
	/// Обработчик HTTP-запросов на базе OWIN для логирования необработанных исключений.
	/// </summary>
	/// <remarks>
	/// Логирует необработанные исключения, возникающие на уровне OWIN.
	/// </remarks>
	sealed class UnhandledExceptionOwinMiddleware : OwinMiddleware
	{
		public UnhandledExceptionOwinMiddleware(OwinMiddleware next, ILog log)
			: base(next)
		{
			if (log == null)
			{
				throw new ArgumentNullException("log");
			}

			_log = log;
		}


		private readonly ILog _log;

		private static readonly Task EmptyTask = Task.FromResult<object>(null);


		public override Task Invoke(IOwinContext context)
		{
			try
			{
				return Next.Invoke(context).ContinueWith(task =>
														 {
															 if (task.IsFaulted)
															 {
																 LogException(task.Exception);
															 }

															 return EmptyTask;
														 });
			}
			catch (Exception exception)
			{
				try
				{
					LogException(exception);

					return EmptyTask;
				}
				catch
				{
					// Не удалось залогировать ошибку
				}

				throw;
			}
		}


		private void LogException(Exception exception)
		{
			_log.Error(Resources.UnhandledExceptionOwinMiddleware, exception);
		}
	}
}