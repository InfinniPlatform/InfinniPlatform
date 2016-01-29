using System;
using System.Collections.Concurrent;

using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.Core.Logging
{
    internal static class LogManagerCache
    {
        private static readonly ConcurrentDictionary<Type, ILog> Logs
            = new ConcurrentDictionary<Type, ILog>();

        public static ILog GetLog(Type type)
        {
            ILog log;

            if (!Logs.TryGetValue(type, out log))
            {
                var internalLog = GetInternalLog(nameof(ILog), type);

                log = new Log4NetLog(internalLog);

                Logs.TryAdd(type, log);
            }

            return log;
        }


        private static readonly ConcurrentDictionary<Type, IPerformanceLog> PerformanceLogs
            = new ConcurrentDictionary<Type, IPerformanceLog>();

        public static IPerformanceLog GetPerformanceLog(Type type)
        {
            IPerformanceLog performanceLog;

            if (!PerformanceLogs.TryGetValue(type, out performanceLog))
            {
                var internalLog = GetInternalLog(nameof(IPerformanceLog), type);

                performanceLog = new Log4NetPerformanceLog(internalLog);

                PerformanceLogs.TryAdd(type, performanceLog);
            }

            return performanceLog;
        }


        private static readonly ConcurrentDictionary<string, log4net.ILog> InternalLogs
            = new ConcurrentDictionary<string, log4net.ILog>();

        private static log4net.ILog GetInternalLog(string prefix, Type type)
        {
            log4net.ILog internalLog;

            var loggerName = $"{prefix}.{type.FullName}";

            if (!InternalLogs.TryGetValue(loggerName, out internalLog))
            {
                internalLog = log4net.LogManager.GetLogger(loggerName);

                InternalLogs.TryAdd(loggerName, internalLog);
            }

            return internalLog;
        }
    }
}