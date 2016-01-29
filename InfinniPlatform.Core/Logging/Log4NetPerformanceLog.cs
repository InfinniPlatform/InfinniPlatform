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

        public void Log(string method, TimeSpan duration, string outcome = null)
        {
            _internalLog.InfoFormat("{0} {1} {2}", method, (long)duration.TotalMilliseconds, outcome ?? "<null>");
        }

        public void Log(string method, DateTime start, string outcome = null)
        {
            Log(method, DateTime.Now - start, outcome);
        }
    }
}