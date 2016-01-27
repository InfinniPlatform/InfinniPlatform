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
            Log = new Log4NetLog(log4net.LogManager.GetLogger("InfinniPlatform"));

            // Интеграция с System.Diagnostics.Trace
            Trace.Listeners.Add(new LogTraceListener(Log));
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