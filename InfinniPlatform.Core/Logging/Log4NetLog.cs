using System;
using System.Collections.Generic;
using System.Security.Principal;

using InfinniPlatform.Auth.Internal.Contract;
using InfinniPlatform.Sdk.Serialization;

using log4net;
using Microsoft.Extensions.Logging;
using ILog = InfinniPlatform.Sdk.Logging.ILog;

namespace InfinniPlatform.Core.Logging
{
    /// <summary>
    /// Сервис <see cref="Sdk.Logging.ILog" /> на базе log4net.
    /// </summary>
    internal sealed class Log4NetLog : ILog, ILogger
    {
        private const string KeyRequestId = "app.RequestId";
        private const string KeyUserId = "app.UserId";
        private const string KeyUserName = "app.UserName";


        public Log4NetLog(log4net.ILog internalLog, IJsonObjectSerializer serializer)
        {
            _internalLog = internalLog;
            _serializer = serializer;
        }


        private readonly log4net.ILog _internalLog;
        private readonly IJsonObjectSerializer _serializer;


        public bool IsDebugEnabled => _internalLog.IsDebugEnabled;

        public bool IsInfoEnabled => _internalLog.IsInfoEnabled;

        public bool IsWarnEnabled => _internalLog.IsWarnEnabled;

        public bool IsErrorEnabled => _internalLog.IsErrorEnabled;

        public bool IsFatalEnabled => _internalLog.IsFatalEnabled;


        public void Debug(string message, Exception exception = null, Func<Dictionary<string, object>> context = null)
        {
            _internalLog.Debug(new LogEvent(message, exception, context, _serializer));
        }

        public void Info(string message, Exception exception = null, Func<Dictionary<string, object>> context = null)
        {
            _internalLog.Info(new LogEvent(message, exception, context, _serializer));
        }

        public void Warn(string message, Exception exception = null, Func<Dictionary<string, object>> context = null)
        {
            _internalLog.Warn(new LogEvent(message, exception, context, _serializer));
        }

        public void Error(string message, Exception exception = null, Func<Dictionary<string, object>> context = null)
        {
            _internalLog.Error(new LogEvent(message, exception, context, _serializer));
        }

        public void Fatal(string message, Exception exception = null, Func<Dictionary<string, object>> context = null)
        {
            _internalLog.Fatal(new LogEvent(message, exception, context, _serializer));
        }


        public void SetRequestId(object requestId)
        {
            ThreadContext.Properties[KeyRequestId] = requestId;
        }

        public void SetUserId(IIdentity user)
        {
            if (user != null)
            {
                ThreadContext.Properties[KeyUserId] = user.GetUserId();
                ThreadContext.Properties[KeyUserName] = user.Name;
            }
            else
            {
                ThreadContext.Properties[KeyUserId] = null;
                ThreadContext.Properties[KeyUserName] = null;
            }
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    _internalLog.Debug(message, exception);
                    break;
                case LogLevel.Information:
                    _internalLog.Info(message, exception);
                    break;
                case LogLevel.Warning:
                    _internalLog.Warn(message, exception);
                    break;
                case LogLevel.Error:
                    _internalLog.Error(message, exception);
                    break;
                case LogLevel.Critical:
                    _internalLog.Fatal(message, exception);
                    break;
                case LogLevel.None:
                    break;
                default:
                    _internalLog.Warn($"Encountered unknown log level {logLevel}, writing out as Info.");
                    _internalLog.Info(message, exception);
                    break;
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return _internalLog.IsDebugEnabled;
                case LogLevel.Information:
                    return _internalLog.IsInfoEnabled;
                case LogLevel.Warning:
                    return _internalLog.IsWarnEnabled;
                case LogLevel.Error:
                    return _internalLog.IsErrorEnabled;
                case LogLevel.Critical:
                    return _internalLog.IsFatalEnabled;
                case LogLevel.None:
                    return false;
                default:
                    throw new ArgumentException($"Unknown log level {logLevel}.", nameof(logLevel));
            }
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }
    }
}