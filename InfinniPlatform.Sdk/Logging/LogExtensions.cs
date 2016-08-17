using System;
using System.Collections.Generic;
using System.Text;

namespace InfinniPlatform.Sdk.Logging
{
    public static class LogExtensions
    {
        /// <summary>
        /// Записывает в журнал событие с уровнем DEBUG.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        public static void Debug(this ILog target, string message, Func<Dictionary<string, object>> context = null)
        {
            target.Debug(message, null, context);
        }

        /// <summary>
        /// Записывает в журнал событие с уровнем DEBUG.
        /// </summary>
        /// <param name="exception">Исключение.</param>
        /// <param name="context">Контекстная информация.</param>
        public static void Debug(this ILog target, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            target.Debug(exception?.Message, exception, context);
        }

        /// <summary>
        /// Записывает в журнал событие с уровнем INFO.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        public static void Info(this ILog target, string message, Func<Dictionary<string, object>> context = null)
        {
            target.Info(message, null, context);
        }
        /// <summary>
        /// Записывает в журнал событие с уровнем INFO.
        /// </summary>
        /// <param name="exception">Исключение.</param>
        /// <param name="context">Контекстная информация.</param>
        public static void Info(this ILog target, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            target.Info(exception?.Message, exception, context);
        }

        /// <summary>
        /// Записывает в журнал событие с уровнем WARN.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        public static void Warn(this ILog target, string message, Func<Dictionary<string, object>> context = null)
        {
            target.Warn(message, null, context);
        }

        /// <summary>
        /// Записывает в журнал событие с уровнем WARN.
        /// </summary>
        /// <param name="exception">Исключение.</param>
        /// <param name="context">Контекстная информация.</param>
        public static void Warn(this ILog target, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            target.Warn(exception?.Message, exception, context);
        }

        /// <summary>
        /// Записывает в журнал событие с уровнем ERROR.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        public static void Error(this ILog target, string message, Func<Dictionary<string, object>> context = null)
        {
            target.Error(message, null, context);
        }
        /// <summary>
        /// Записывает в журнал событие с уровнем ERROR.
        /// </summary>
        /// <param name="exception">Исключение.</param>
        /// <param name="context">Контекстная информация.</param>
        public static void Error(this ILog target, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            target.Error(exception?.Message, exception, context);
        }

        /// <summary>
        /// Записывает в журнал событие с уровнем FATAL.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="context">Контекстная информация.</param>
        public static void Fatal(this ILog target, string message, Func<Dictionary<string, object>> context = null)
        {
            target.Fatal(message, null, context);
        }
        /// <summary>
        /// Записывает в журнал событие с уровнем FATAL.
        /// </summary>
        /// <param name="exception">Исключение.</param>
        /// <param name="context">Контекстная информация.</param>
        public static void Fatal(this ILog target, Exception exception, Func<Dictionary<string, object>> context = null)
        {
            target.Fatal(exception?.Message, exception, context);
        }


        /// <summary>
        /// Фиксирует в логе информацию о длительности вызова указанного метода.
        /// </summary>
        /// <param name="method">Метод компонента, длительность вызова которого была замерена.</param>
        /// <param name="duration">Длительность выполнения метода.</param>
        /// <param name="outcome">Результат выполнения: <c>null</c> - если вызов метода завершился успешно; <c>текст исключения</c> - иначе.</param>
        public static void Log(this IPerformanceLog target, string method, TimeSpan duration, Exception outcome)
        {
            target.Log(method, duration, GetFullMessage(outcome));
        }

        /// <summary>
        /// Фиксирует в логе информацию о длительности работы указанного метода, используя <c>DateTime.Now</c> в качестве момента окончания выполнения метода.
        /// </summary>
        /// <param name="method">Метод компонента, длительность вызова которого была замерена.</param>
        /// <param name="start">Момент начала выполнения метода.</param>
        /// <param name="outcome">Результат выполнения: <c>null</c> - если вызов метода завершился успешно; <c>текст исключения</c> - иначе.</param>
        public static void Log(this IPerformanceLog target, string method, DateTime start, Exception outcome)
        {
            target.Log(method, start, GetFullMessage(outcome));
        }


        /// <summary>
        /// Возвращает полное сообщение исключения, включая все вложенные.
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
        /// Возвращает полный стек вызов исключения, включая все вложенные.
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
    }
}