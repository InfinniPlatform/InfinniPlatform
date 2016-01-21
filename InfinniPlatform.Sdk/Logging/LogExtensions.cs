using System;
using System.Text;

namespace InfinniPlatform.Sdk.Logging
{
    public static class LogExtensions
    {
        /// <summary>
        /// Фиксирует в логе информацию о длительности вызова указанного метода.
        /// </summary>
        /// <param name="component">Компонент, чей метод вызван.</param>
        /// <param name="method">Метод компонента, длительность вызова которого была замерена.</param>
        /// <param name="duration">Длительность выполнения метода.</param>
        /// <param name="outcome">Результат выполнения: <c>null</c> - если вызов метода завершился успешно; <c>текст исключения</c> - иначе.</param>
        public static void Log(this IPerformanceLog target, string component, string method, TimeSpan duration, Exception outcome)
        {
            target.Log(component, method, duration, GetMessage(outcome));
        }

        /// <summary>
        /// Фиксирует в логе информацию о длительности работы указанного метода, используя <c>DateTime.Now</c> в качестве момента окончания выполнения метода.
        /// </summary>
        /// <param name="component">Компонент, чей метод вызван.</param>
        /// <param name="method">Метод компонента, длительность вызова которого была замерена.</param>
        /// <param name="start">Момент начала выполнения метода.</param>
        /// <param name="outcome">Результат выполнения: <c>null</c> - если вызов метода завершился успешно; <c>текст исключения</c> - иначе.</param>
        public static void Log(this IPerformanceLog target, string component, string method, DateTime start, Exception outcome)
        {
            target.Log(component, method, start, GetMessage(outcome));
        }

        /// <summary>
        /// Возвращает полное сообщение об ошибке, включая все вложенные.
        /// </summary>
        public static string GetMessage(this Exception exception)
        {
            if (exception != null)
            {
                var message = new StringBuilder();
                GetMessage(exception, message);
                return message.ToString();
            }

            return null;
        }

        private static void GetMessage(Exception exception, StringBuilder message)
        {
            message.AppendLine(exception.Message);

            var aggregateException = exception as AggregateException;

            if (aggregateException != null)
            {
                if (aggregateException.InnerExceptions != null && aggregateException.InnerExceptions.Count > 0)
                {
                    foreach (var innerException in aggregateException.InnerExceptions)
                    {
                        GetMessage(innerException, message);
                    }
                }
            }
            else if (exception.InnerException != null)
            {
                GetMessage(exception.InnerException, message);
            }
        }
    }
}