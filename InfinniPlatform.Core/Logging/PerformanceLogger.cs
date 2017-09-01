using System;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Logging
{
    public class PerformanceLogger<TComponent> : IPerformanceLogger<TComponent>
    {
        public PerformanceLogger(ILogger<IPerformanceLogger> logger)
        {
            _logger = logger;
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

            _logger.Log(LogLevel.Information, 0, new PerformanceLoggerEvent(method, duration, typeof(TComponent)), exception, MessageFormatter);
        }


        private class PerformanceLoggerEvent
        {
            public PerformanceLoggerEvent(string method, double duration, Type componentType)
            {
                _method = method;
                _duration = duration;
                _componentType = componentType;
            }


            private readonly string _method;
            private readonly double _duration;
            private readonly Type _componentType;


            public override string ToString()
            {
                var component = LoggerNameHelper.GetCategoryName(_componentType);

                return string.Format("{{ \"m\":\"{0}\", \"d\": {1:N0}, \"component\":\"{2}\" }}", _method, _duration, component);
            }
        }
    }
}