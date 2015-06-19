using InfinniPlatform.Api.Profiling;

namespace InfinniPlatform.Logging
{
    /// <summary>
    ///     Предоставляет статическую точку доступа к <see cref="ILog" />.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        ///     Сервис журналирования событий.
        /// </summary>
        public static readonly ILog Log = new Log4NetLogFactory().CreateLog();
    }
}