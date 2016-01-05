using System;
using System.Collections.Concurrent;

using log4net;

namespace InfinniPlatform.Core.Logging
{
    internal static class Log4NetLogManagerCache
    {
        private static readonly ConcurrentDictionary<Type, ILog> Logs
            = new ConcurrentDictionary<Type, ILog>();

        public static ILog GetLog(Type type)
        {
            ILog log;

            if (!Logs.TryGetValue(type, out log))
            {
                log = LogManager.GetLogger(type);

                Logs.TryAdd(type, log);
            }

            return log;
        }
    }
}