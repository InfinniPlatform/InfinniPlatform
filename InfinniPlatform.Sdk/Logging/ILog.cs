using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace InfinniPlatform.Sdk.Logging
{
    /// <summary>
    /// Сервис регистрации событий.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Разрешены ли события с уровнем DEBUG.
        /// </summary>
        bool IsDebugEnabled { get; }

        /// <summary>
        /// Разрешены ли события с уровнем INFO.
        /// </summary>
        bool IsInfoEnabled { get; }

        /// <summary>
        /// Разрешены ли события с уровнем WARN.
        /// </summary>
        bool IsWarnEnabled { get; }

        /// <summary>
        /// Разрешены ли события с уровнем ERROR.
        /// </summary>
        bool IsErrorEnabled { get; }

        /// <summary>
        /// Разрешены ли события с уровнем FATAL.
        /// </summary>
        bool IsFatalEnabled { get; }

        /// <summary>
        /// Записывает в журнал событие с уровнем DEBUG.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        /// <param name="exception">Исключение.</param>
        void Debug(object message, Dictionary<string, object> context = null, Exception exception = null);

        /// <summary>
        /// Записывает в журнал событие с уровнем INFO.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        /// <param name="exception">Исключение.</param>
        void Info(object message, Dictionary<string, object> context = null, Exception exception = null);

        /// <summary>
        /// Записывает в журнал событие с уровнем WARN.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        /// <param name="exception">Исключение.</param>
        void Warn(object message, Dictionary<string, object> context = null, Exception exception = null);

        /// <summary>
        /// Записывает в журнал событие с уровнем ERROR.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        /// <param name="exception">Исключение.</param>
        void Error(object message, Dictionary<string, object> context = null, Exception exception = null);

        /// <summary>
        /// Записывает в журнал событие с уровнем FATAL.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        /// <param name="exception">Исключение.</param>
        void Fatal(object message, Dictionary<string, object> context = null, Exception exception = null);

        /// <summary>
        /// Устанавливает идентификатор запроса в контекст логирования текущего потока.
        /// </summary>
        /// <param name="context">Контекстные данные.</param>
        void InitThreadLoggingContext(IDictionary<string, object> context);

        void SetContext(IDictionary<string, object> context);

        void SetUserId(IIdentity user);
    }
}