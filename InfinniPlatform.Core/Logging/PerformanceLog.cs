using System;

using InfinniPlatform.Sdk.Environment.Log;

using log4net;

using ILog4NetLog = log4net.ILog;

namespace InfinniPlatform.Logging
{
    public class PerformanceLog : IPerformanceLog
    {
        // TODO: Refactor: inject ILog via DI
        private static readonly log4net.ILog Logger = LogManager.GetLogger(typeof(PerformanceLog));

        public void Log(string component, string method, TimeSpan duration, string outcome)
        {
            Logger.InfoFormat("{0} {1} {2} {3}", component, method, (long) duration.TotalMilliseconds, outcome ?? "<null>");
        }

        public void Log(string component, string method, DateTime start, string outcome)
        {
            Log(component, method, DateTime.Now - start, outcome);
        }
    }
}