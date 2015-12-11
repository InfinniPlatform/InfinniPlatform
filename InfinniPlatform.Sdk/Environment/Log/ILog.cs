using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Environment.Log
{
    /// <summary>
    /// Сервис регистрации событий.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Записывает в журнал событие с уровнем INFO.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        /// <param name="exception">Исключение.</param>
        void Info(string message, Dictionary<string, Object> context = null, Exception exception = null);

        /// <summary>
        /// Записывает в журнал событие с уровнем WARN.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        /// <param name="exception">Исключение.</param>
        void Warn(string message, Dictionary<string, Object> context = null, Exception exception = null);

        /// <summary>
        /// Записывает в журнал событие с уровнем DEBUG.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        /// <param name="exception">Исключение.</param>
        void Debug(string message, Dictionary<string, Object> context = null, Exception exception = null);

        /// <summary>
        /// Записывает в журнал событие с уровнем ERROR.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        /// <param name="exception">Исключение.</param>
        void Error(string message, Dictionary<string, Object> context = null, Exception exception = null);

        /// <summary>
        /// Записывает в журнал событие с уровнем FATAL.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        /// <param name="exception">Исключение.</param>
        void Fatal(string message, Dictionary<string, Object> context = null, Exception exception = null);

        /// <summary>
        /// Инициализирует контекст логирования текущего потока информацией из словаря.
        /// </summary>
        /// <param name="context">Контекстные данные.</param>
        void InitThreadLoggingContext(IDictionary<string, object> context);
    }
}