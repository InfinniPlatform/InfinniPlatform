using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace InfinniPlatform.Core.Abstractions.Logging
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
        /// <param name="exception">Исключение.</param>
        /// <param name="context">Контекстная информация.</param>
        void Debug(string message, Exception exception = null, Func<Dictionary<string, object>> context = null);

        /// <summary>
        /// Записывает в журнал событие с уровнем INFO.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="exception">Исключение.</param>
        /// <param name="context">Контекстная информация.</param>
        void Info(string message, Exception exception = null, Func<Dictionary<string, object>> context = null);

        /// <summary>
        /// Записывает в журнал событие с уровнем WARN.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="exception">Исключение.</param>
        /// <param name="context">Контекстная информация.</param>
        void Warn(string message, Exception exception = null, Func<Dictionary<string, object>> context = null);

        /// <summary>
        /// Записывает в журнал событие с уровнем ERROR.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="exception">Исключение.</param>
        /// <param name="context">Контекстная информация.</param>
        void Error(string message, Exception exception = null, Func<Dictionary<string, object>> context = null);

        /// <summary>
        /// Записывает в журнал событие с уровнем FATAL.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="exception">Исключение.</param>
        /// <param name="context">Контекстная информация.</param>
        void Fatal(string message, Exception exception = null, Func<Dictionary<string, object>> context = null);


        /// <summary>
        /// Устанавливает идентификатор запроса в контекст логирования текущего потока.
        /// </summary>
        /// <param name="requestId">Идентификатор запроса.</param>
        void SetRequestId(object requestId);

        /// <summary>
        /// Устанавливает идентификатор пользователя в контекст логирования текущего потока.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        void SetUserId(IIdentity user);
    }
}