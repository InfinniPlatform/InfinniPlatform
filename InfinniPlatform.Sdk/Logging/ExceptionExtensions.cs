using System;
using System.Text;

namespace InfinniPlatform.Sdk.Logging
{
    public static class ExceptionExtensions
    {
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