using System;

using InfinniPlatform.Sdk.Logging;

using ILog = log4net.ILog;

namespace InfinniPlatform.Core.Logging
{
    public class PerformanceLog : IPerformanceLog
    {
        public PerformanceLog(ILog log)
        {
            _log = log;
        }

        private readonly ILog _log;

        public void Log(string component, string method, TimeSpan duration, string outcome)
        {
            _log.InfoFormat("{0} {1} {2} {3}", component, method, (long)duration.TotalMilliseconds, outcome ?? "<null>");
        }

        public void Log(string component, string method, DateTime start, string outcome)
        {
            Log(component, method, DateTime.Now - start, outcome);
        }
    }
}