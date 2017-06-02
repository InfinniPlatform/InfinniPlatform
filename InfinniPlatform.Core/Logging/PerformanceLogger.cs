using System;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Logging
{
    public class PerformanceLogger<TComponent> : IPerformanceLogger<TComponent>
    {
        public PerformanceLogger(AppOptions options, ILoggerFactory loggerFactory)
        {
            var loggerCategoryName = LoggerNameHelper.GetCategoryName(typeof(TComponent));

            _logger = loggerFactory.CreateLogger($"{options.PerformanceLoggerNamePrefix}{loggerCategoryName}");
        }


        private readonly ILogger _logger;


        public void Log(string method, TimeSpan duration, Exception exception = null)
        {
            LogInternal(method, duration.TotalMilliseconds, exception);
        }

        public void Log(string method, DateTime start, Exception exception = null)
        {
            LogInternal(method, (DateTime.Now - start).TotalMilliseconds, exception);
        }

        private void LogInternal(string method, double duration, Exception exception)
        {
            string MessageFormatter(object m, Exception e) => m.ToString();

            _logger.Log(LogLevel.Information, 0, new PerformanceLoggerEvent(method, duration), exception, MessageFormatter);
        }


        private class PerformanceLoggerEvent
        {
            public PerformanceLoggerEvent(string method, double duration)
            {
                _method = method;
                _duration = duration;
            }


            private readonly string _method;
            private readonly double _duration;


            public override string ToString()
            {
                return $"{_method}={_duration:N0}";
            }
        }
    }
}