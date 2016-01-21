using log4net;

using ILog = InfinniPlatform.Sdk.Logging.ILog;

namespace InfinniPlatform.Core.Logging
{
    /// <summary>
    /// Предоставляет статическую точку доступа к <see cref="ILog" />.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Сервис регистрации событий.
        /// </summary>
        /// <remarks>
        /// Используется только в статическом контексте, где недоступен IoC.
        /// </remarks>
        public static readonly ILog Log = new Log4NetLog(LogManager.GetLogger("InfinniPlatform"));
    }
}