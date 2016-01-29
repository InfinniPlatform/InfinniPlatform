using System.Diagnostics;

using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.Core.Logging
{
    /// <summary>
    /// Предоставляет статическую точку доступа к <see cref="ILog" />.
    /// </summary>
    public static class Logger
    {
        static Logger()
        {
            Log = LogManagerCache.GetLog(typeof(Logger));

            // Интеграция с System.Diagnostics.Trace
            var traceLog = LogManagerCache.GetLog(typeof(LogTraceListener));
            Trace.Listeners.Add(new LogTraceListener(traceLog));
        }

        /// <summary>
        /// Сервис регистрации событий.
        /// </summary>
        /// <remarks>
        /// Используется только в статическом контексте, где недоступен IoC.
        /// </remarks>
        public static readonly ILog Log;
    }
}