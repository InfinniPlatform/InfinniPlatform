using System;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Logging
{
    /// <inheritdoc />
    public class PerformanceLogger<TComponent> : IPerformanceLogger<TComponent>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PerformanceLogger{TComponent}" />.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public PerformanceLogger(ILogger<IPerformanceLogger> logger)
        {
            _logger = logger;
            _component = LoggerNameHelper.GetCategoryName(typeof(TComponent));
        }


        private readonly ILogger _logger;
        private readonly string _component;


        /// <inheritdoc />
        public void Log(string method, TimeSpan duration, Exception exception = null)
        {
            LogInternal(method, duration.TotalMilliseconds, exception);
        }

        /// <inheritdoc />
        public void Log(string method, DateTime start, Exception exception = null)
        {
            LogInternal(method, (DateTime.Now - start).TotalMilliseconds, exception);
        }

        private void LogInternal(string method, double duration, Exception exception)
        {
            string MessageFormatter(object m, Exception e) => m.ToString();

            _logger.Log(LogLevel.Information, 0, new PerformanceLoggerEvent(method, duration, _component), exception, MessageFormatter);
        }


        private class PerformanceLoggerEvent
        {
            public PerformanceLoggerEvent(string method, double duration, string component)
            {
                _method = method;
                _duration = duration;
                _component = component;
            }


            private readonly string _method;
            private readonly double _duration;
            private readonly string _component;


            public override string ToString()
            {
                return string.Format("{{ \"m\":\"{0}\", \"d\": {1:N0}, \"component\":\"{2}\" }}", _method, _duration, _component);
            }
        }
    }
}