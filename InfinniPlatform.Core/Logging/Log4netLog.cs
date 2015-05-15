using System;
using InfinniPlatform.Api.Profiling;
using ILog4NetLog = log4net.ILog;

namespace InfinniPlatform.Logging
{
	/// <summary>
	/// Сервис <see cref="ILog"/> на базе log4net.
	/// </summary>
	public sealed class Log4NetLog : ILog
	{
		private readonly ILog4NetLog _log;

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="log">Сервис log4net для записи сообщений в лог.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public Log4NetLog(ILog4NetLog log)
		{
			if (log == null)
			{
				throw new ArgumentNullException();
			}

			_log = log;
		}


		/// <summary>
		/// Записывает в журнал информационное сообщение.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="exception">Произошедшее исключение.</param>
		public void Info(string message, Exception exception)
		{
			_log.Info(message, exception);
		}

		/// <summary>
		/// Записывает в журнал информационное сообщение.
		/// </summary>
		/// <param name="formatMessage">Строка форматирования сообщения.</param>
		/// <param name="formatArguments">Список параметров для строки форматирования сообщения.</param>
		public void Info(string formatMessage, params object[] formatArguments)
		{
			_log.InfoFormat(formatMessage, formatArguments);
		}


		/// <summary>
		/// Записывает в журнал сообщение с предупреждением.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="exception">Произошедшее исключение.</param>
		public void Warn(string message, Exception exception)
		{
			_log.Warn(message, exception);
		}

		/// <summary>
		/// Записывает в журнал сообщение с предупреждением.
		/// </summary>
		/// <param name="formatMessage">Строка форматирования сообщения.</param>
		/// <param name="formatArguments">Список параметров для строки форматирования сообщения.</param>
		public void Warn(string formatMessage, params object[] formatArguments)
		{
			_log.WarnFormat(formatMessage, formatArguments);
		}


		/// <summary>
		/// Записывает в журнал сообщение с ошибкой.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="exception">Произошедшее исключение.</param>
		public void Error(string message, Exception exception)
		{
			_log.Error(message, exception);
		}

		/// <summary>
		/// Записывает в журнал сообщение с ошибкой.
		/// </summary>
		/// <param name="formatMessage">Строка форматирования сообщения.</param>
		/// <param name="formatArguments">Список параметров для строки форматирования сообщения.</param>
		public void Error(string formatMessage, params object[] formatArguments)
		{
			_log.ErrorFormat(formatMessage, formatArguments);
		}


		/// <summary>
		/// Записывает в журнал сообщение с критичной ошибкой.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="exception">Произошедшее исключение.</param>
		public void Fatal(string message, Exception exception)
		{
			_log.Fatal(message, exception);
		}

		/// <summary>
		/// Записывает в журнал сообщение с критичной ошибкой.
		/// </summary>
		/// <param name="formatMessage">Строка форматирования сообщения.</param>
		/// <param name="formatArguments">Список параметров для строки форматирования сообщения.</param>
		public void Fatal(string formatMessage, params object[] formatArguments)
		{
			_log.FatalFormat(formatMessage, formatArguments);
		}
	}
}