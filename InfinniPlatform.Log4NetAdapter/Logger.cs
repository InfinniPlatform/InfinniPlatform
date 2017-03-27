using System.Diagnostics;

using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Log4NetAdapter
{
    /// <summary>
    /// Предоставляет статическую точку доступа к <see cref="ILog" />.
    /// </summary>
    public static class Logger
    {
        static Logger()
        {
            // Объект для сериализации событий вне контекста IoC
            var serializer = JsonObjectSerializer.Default;

            // Статический сервис регистрации событий вне контекста IoC
            Log = LogManagerCache.GetLog(typeof(Logger), serializer);

            // Реализация интеграции с System.Diagnostics.Trace
            var traceLog = LogManagerCache.GetLog(typeof(LogTraceListener), serializer);
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