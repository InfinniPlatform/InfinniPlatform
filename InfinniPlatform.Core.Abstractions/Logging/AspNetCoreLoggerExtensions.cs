using System;
using System.Collections.Generic;
using System.Text;

// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.Logging
{
    public static class AspNetCoreLoggerExtensions
    {
        private static readonly Func<object, Exception, string> MessageFormatter = (message, exception) => message.ToString();


        /// <summary>
        /// Formats and writes a debug log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogDebug(this ILogger logger, string message, Func<Dictionary<string, object>> context)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            LogDebug(logger, message, null, context);
        }

        /// <summary>
        /// Formats and writes a debug log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogDebug(this ILogger logger, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            LogDebug(logger, exception.Message, exception, context);
        }

        /// <summary>
        /// Formats and writes a debug log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogDebug(this ILogger logger, string message, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Debug, 0, new LoggerEvent(message, context), exception, MessageFormatter);
        }


        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogInformation(this ILogger logger, string message, Func<Dictionary<string, object>> context)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            LogInformation(logger, message, null, context);
        }

        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogInformation(this ILogger logger, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            LogInformation(logger, exception.Message, exception, context);
        }

        /// <summary>
        /// Formats and writes an informational log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogInformation(this ILogger logger, string message, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Information, 0, new LoggerEvent(message, context), exception, MessageFormatter);
        }


        /// <summary>
        /// Formats and writes a warning log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogWarning(this ILogger logger, string message, Func<Dictionary<string, object>> context)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            LogWarning(logger, message, null, context);
        }

        /// <summary>
        /// Formats and writes a warning log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogWarning(this ILogger logger, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            LogWarning(logger, exception.Message, exception, context);
        }

        /// <summary>
        /// Formats and writes a warning log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogWarning(this ILogger logger, string message, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Warning, 0, new LoggerEvent(message, context), exception, MessageFormatter);
        }


        /// <summary>
        /// Formats and writes an error log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogError(this ILogger logger, string message, Func<Dictionary<string, object>> context)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            LogError(logger, message, null, context);
        }

        /// <summary>
        /// Formats and writes an error log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogError(this ILogger logger, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            LogError(logger, exception.Message, exception, context);
        }

        /// <summary>
        /// Formats and writes an error log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogError(this ILogger logger, string message, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Error, 0, new LoggerEvent(message, context), exception, MessageFormatter);
        }


        /// <summary>
        /// Formats and writes a critical log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogCritical(this ILogger logger, string message, Func<Dictionary<string, object>> context)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            LogCritical(logger, message, null, context);
        }

        /// <summary>
        /// Formats and writes a critical log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogCritical(this ILogger logger, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            LogCritical(logger, exception.Message, exception, context);
        }

        /// <summary>
        /// Formats and writes a critical log message.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/> to write to.</param>
        /// <param name="message">The message to log.</param>
        /// <param name="exception">The exception to log.</param>
        /// <param name="context">The contextual information to log.</param>
        public static void LogCritical(this ILogger logger, string message, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.Log(LogLevel.Critical, 0, new LoggerEvent(message, context), exception, MessageFormatter);
        }


        /// <summary>
        /// Gets a full message of the exception, including its inner exceptions.
        /// </summary>
        public static string GetFullMessage(this Exception exception)
        {
            if (exception != null)
            {
                var message = new StringBuilder();
                GetFullMessage(exception, message, 0);
                return message.ToString();
            }

            return null;
        }

        private static void GetFullMessage(Exception exception, StringBuilder message, int nested)
        {
            if (nested > 0)
            {
                message.AppendLine().Append(new string('-', nested)).Append("> ");
            }

            message.Append(exception.GetType().FullName).Append(": ").Append(exception.Message);

            var aggregateException = exception as AggregateException;

            if (aggregateException != null)
            {
                if (aggregateException.InnerExceptions != null && aggregateException.InnerExceptions.Count > 0)
                {
                    foreach (var innerException in aggregateException.InnerExceptions)
                    {
                        GetFullMessage(innerException, message, nested + 1);
                    }
                }
            }
            else if (exception.InnerException != null)
            {
                GetFullMessage(exception.InnerException, message, nested + 1);
            }
        }


        /// <summary>
        /// Gets a full stack trace of the exception, including its inner exceptions.
        /// </summary>
        public static string GetFullStackTrace(this Exception exception)
        {
            if (exception != null)
            {
                var stackTrace = new StringBuilder();
                GetFullStackTrace(exception, stackTrace, 0);
                return stackTrace.ToString();
            }

            return null;
        }

        private static void GetFullStackTrace(Exception exception, StringBuilder stackTrace, int nested)
        {
            if (nested > 0)
            {
                stackTrace.AppendLine().Append(new string('-', nested)).Append("> ");
            }

            stackTrace.Append(exception.GetType().FullName).Append(": ").AppendLine().Append(exception.StackTrace ?? "null");

            var aggregateException = exception as AggregateException;

            if (aggregateException != null)
            {
                if (aggregateException.InnerExceptions != null && aggregateException.InnerExceptions.Count > 0)
                {
                    foreach (var innerException in aggregateException.InnerExceptions)
                    {
                        GetFullStackTrace(innerException, stackTrace, nested + 1);
                    }
                }
            }
            else if (exception.InnerException != null)
            {
                GetFullStackTrace(exception.InnerException, stackTrace, nested + 1);
            }
        }


        private class LoggerEvent
        {
            public LoggerEvent(string message, Func<Dictionary<string, object>> context)
            {
                _message = message;
                _context = context;
            }


            private readonly string _message;
            private readonly Func<Dictionary<string, object>> _context;


            public override string ToString()
            {
                if (_context == null)
                {
                    return _message;
                }

                var result = new StringBuilder(_message);

                try
                {
                    var items = _context();

                    if (items != null)
                    {
                        result.Append(" Context:");

                        foreach (var item in items)
                        {
                            result.Append(' ').Append(item.Key).Append('=').Append(item.Value).Append(';');
                        }
                    }
                }
                catch
                {
                    // ignore
                }

                return result.ToString();
            }
        }
    }
}