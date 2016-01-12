using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Logging;

using log4net;

using ILog = InfinniPlatform.Sdk.Logging.ILog;
using ILog4NetLog = log4net.ILog;

namespace InfinniPlatform.Core.Logging
{
    /// <summary>
    /// Сервис <see cref="ILog" /> на базе log4net.
    /// </summary>
    public sealed class Log4NetLog : Sdk.Logging.ILog
    {
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

        private readonly ILog4NetLog _log;

        public void Info(string message, Dictionary<string, object> context, Exception exception = null)
        {
            _log.Info(new JsonEvent(message, context, exception));
        }

        public void Warn(string message, Dictionary<string, object> context, Exception exception = null)
        {
            _log.Warn(new JsonEvent(message, context, exception));
        }

        public void Debug(string message, Dictionary<string, object> context, Exception exception = null)
        {
            _log.Debug(new JsonEvent(message, context, exception));
        }

        public void Error(string message, Dictionary<string, object> context, Exception exception = null)
        {
            _log.Error(new JsonEvent(message, context, exception));
        }

        public void Fatal(string message, Dictionary<string, object> context, Exception exception = null)
        {
            _log.Fatal(new JsonEvent(message, context, exception));
        }

        public void InitThreadLoggingContext(IDictionary<string, object> context)
        {
            ThreadContext.Properties.Clear();

            foreach (var pair in context)
            {
                ThreadContext.Properties[pair.Key] = pair.Value;
            }
        }
    }
}