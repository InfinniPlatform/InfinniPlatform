using System;

namespace InfinniPlatform.Logging
{
	/// <summary>
	/// Сервис журналирования событий.
	/// </summary>
	public interface ILog
	{
		/// <summary>
		/// Записывает в журнал информационное сообщение.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="exception">Произошедшее исключение.</param>
		void Info(string message, Exception exception);

		/// <summary>
		/// Записывает в журнал информационное сообщение.
		/// </summary>
		/// <param name="formatMessage">Строка форматирования сообщения.</param>
		/// <param name="formatArguments">Список параметров для строки форматирования сообщения.</param>
		void Info(string formatMessage, params object[] formatArguments);


		/// <summary>
		/// Записывает в журнал сообщение с предупреждением.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="exception">Произошедшее исключение.</param>
		void Warn(string message, Exception exception);

		/// <summary>
		/// Записывает в журнал сообщение с предупреждением.
		/// </summary>
		/// <param name="formatMessage">Строка форматирования сообщения.</param>
		/// <param name="formatArguments">Список параметров для строки форматирования сообщения.</param>
		void Warn(string formatMessage, params object[] formatArguments);


		/// <summary>
		/// Записывает в журнал сообщение с ошибкой.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="exception">Произошедшее исключение.</param>
		void Error(string message, Exception exception);

		/// <summary>
		/// Записывает в журнал сообщение с ошибкой.
		/// </summary>
		/// <param name="formatMessage">Строка форматирования сообщения.</param>
		/// <param name="formatArguments">Список параметров для строки форматирования сообщения.</param>
		void Error(string formatMessage, params object[] formatArguments);


		/// <summary>
		/// Записывает в журнал сообщение с критичной ошибкой.
		/// </summary>
		/// <param name="message">Текст сообщения.</param>
		/// <param name="exception">Произошедшее исключение.</param>
		void Fatal(string message, Exception exception);

		/// <summary>
		/// Записывает в журнал сообщение с критичной ошибкой.
		/// </summary>
		/// <param name="formatMessage">Строка форматирования сообщения.</param>
		/// <param name="formatArguments">Список параметров для строки форматирования сообщения.</param>
		void Fatal(string formatMessage, params object[] formatArguments);
	}
}