using log4net;

using ILog = InfinniPlatform.Sdk.Environment.Log.ILog;

namespace InfinniPlatform.Core.Logging
{
    /// <summary>
    /// Предоставляет статическую точку доступа к <see cref="Sdk.Environment.Log.ILog" />.
    /// </summary>
    public static class Logger
    {
        // TODO: Перевести на IoC

        /// <summary>
        /// Сервис регистрации событий.
        /// </summary>
        public static readonly ILog Log = new Log4NetLog(LogManager.GetLogger("InfinniPlatform"));
    }
}