using System;
using System.Collections;
using System.Text;

namespace InfinniPlatform.Sdk.Environment.Log
{
    public static class ExceptionExtensions
    {
        public static T AddContextData<T>(this T exception, IDictionary contextData) where T : Exception
        {
            foreach (var key in contextData.Keys)
            {
                exception.Data.Add(key, contextData[key]);
            }

            return exception;
        }

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

            if (aggregateException?.InnerExceptions != null && aggregateException.InnerExceptions.Count > 0)
            {
                foreach (var innerException in aggregateException.InnerExceptions)
                {
                    GetMessage(innerException, message);
                }
            }
        }
    }
}