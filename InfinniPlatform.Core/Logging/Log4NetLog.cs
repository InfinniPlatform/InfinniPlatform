using System;
using System.Collections.Generic;
using System.Security.Principal;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.Core.Logging
{
    /// <summary>
    /// Сервис <see cref="ILog" /> на базе log4net.
    /// </summary>
    internal sealed class Log4NetLog : ILog
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


        public void Info(object message, Dictionary<string, object> context = null, Exception exception = null)
        {
            _log.Info(new JsonEvent(message, context, exception));
        }

        public void Warn(object message, Dictionary<string, object> context = null, Exception exception = null)
        {
            _log.Warn(new JsonEvent(message, context, exception));
        }

        public void Debug(object message, Dictionary<string, object> context = null, Exception exception = null)
        {
            _log.Debug(new JsonEvent(message, context, exception));
        }

        public void Error(object message, Dictionary<string, object> context = null, Exception exception = null)
        {
            _log.Error(new JsonEvent(message, context, exception));
        }

        public void Fatal(object message, Dictionary<string, object> context = null, Exception exception = null)
        {
            _log.Fatal(new JsonEvent(message, context, exception));
        }


        public void InitThreadLoggingContext(IIdentity user, IDictionary<string, object> context)
        {
            log4net.ThreadContext.Properties.Clear();

            foreach (var pair in context)
            {
                log4net.ThreadContext.Properties[pair.Key] = pair.Value;
            }

            if (user != null)
            {
                log4net.ThreadContext.Properties["app.UserId"] = user.GetUserId();
                log4net.ThreadContext.Properties["app.UserName"] = user.Name;
            }
        }
    }
}