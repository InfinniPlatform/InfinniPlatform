using System;
using System.Collections.Concurrent;
using System.Reflection;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Core.Logging
{
    internal static class LogManagerCache
    {
        static LogManagerCache()
        {
            log4net.Util.SystemInfo.NullText = "null";
        }


        private static readonly ConcurrentDictionary<Type, ILog> Logs
            = new ConcurrentDictionary<Type, ILog>();

        public static ILog GetLog(Type type, IJsonObjectSerializer serializer)
        {
            ILog log;

            if (!Logs.TryGetValue(type, out log))
            {
                var internalLog = GetInternalLog(nameof(ILog), type);

                log = new Log4NetLog(internalLog, serializer);

                Logs.TryAdd(type, log);
            }

            return log;
        }


        private static readonly ConcurrentDictionary<Type, IPerformanceLog> PerformanceLogs
            = new ConcurrentDictionary<Type, IPerformanceLog>();

        public static IPerformanceLog GetPerformanceLog(Type type, IJsonObjectSerializer serializer)
        {
            IPerformanceLog performanceLog;

            if (!PerformanceLogs.TryGetValue(type, out performanceLog))
            {
                var internalLog = GetInternalLog(nameof(IPerformanceLog), type);

                performanceLog = new Log4NetPerformanceLog(internalLog, serializer);

                PerformanceLogs.TryAdd(type, performanceLog);
            }

            return performanceLog;
        }


        private static readonly ConcurrentDictionary<string, log4net.ILog> InternalLogs
            = new ConcurrentDictionary<string, log4net.ILog>();

        private static log4net.ILog GetInternalLog(string prefix, Type type)
        {
            log4net.ILog internalLog;

            var loggerName = GetInternalLoggerName(prefix, type);

            if (!InternalLogs.TryGetValue(loggerName, out internalLog))
            {
                internalLog = log4net.LogManager.GetLogger(loggerName);

                InternalLogs.TryAdd(loggerName, internalLog);
            }

            return internalLog;
        }

        private static string GetInternalLoggerName(string prefix, Type type)
        {
            var loggerInfo = type.GetCustomAttribute<LoggerNameAttribute>();

            return !string.IsNullOrWhiteSpace(loggerInfo?.Name)
                       ? $"{prefix}.{loggerInfo.Name.Trim()}"
                       : $"{prefix}.{type.FullName}";
        }
    }
}