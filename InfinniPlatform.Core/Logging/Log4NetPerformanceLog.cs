using System;

using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.Core.Logging
{
    /// <summary>
    /// Сервис <see cref="IPerformanceLog" /> на базе log4net.
    /// </summary>
    internal sealed class Log4NetPerformanceLog : IPerformanceLog
    {
        public Log4NetPerformanceLog(log4net.ILog internalLog)
        {
            _internalLog = internalLog;
        }

        private readonly log4net.ILog _internalLog;

        public void Log(string method, TimeSpan duration, Exception exception = null)
        {
            _internalLog.Info(new LogEventPerformance(method, duration.TotalMilliseconds, exception));
        }

        public void Log(string method, DateTime start, Exception exception = null)
        {
            Log(method, DateTime.Now - start, exception);
        }
    }
}