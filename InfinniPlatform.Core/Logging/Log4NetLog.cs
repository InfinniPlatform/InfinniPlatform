using System;
using System.Collections.Generic;
using System.Security.Principal;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Security;

using log4net;

namespace InfinniPlatform.Core.Logging
{
    /// <summary>
    /// Сервис <see cref="Sdk.Logging.ILog" /> на базе log4net.
    /// </summary>
    internal sealed class Log4NetLog : Sdk.Logging.ILog
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="log">Сервис log4net для записи сообщений в лог.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Log4NetLog(log4net.ILog log)
        {
            if (log == null)
            {
                throw new ArgumentNullException();
            }

            _log = log;
        }

        private readonly log4net.ILog _log;

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

        public void InitThreadLoggingContext(IIdentity user, IDictionary<string, object> context)
        {
            ThreadContext.Properties.Clear();

            foreach (var pair in context)
            {
                ThreadContext.Properties[pair.Key] = pair.Value;
            }

            if (user != null)
            {
                ThreadContext.Properties["app.UserId"] = user.GetUserId();
                ThreadContext.Properties["app.UserName"] = user.Name;
            }
        }
    }
}