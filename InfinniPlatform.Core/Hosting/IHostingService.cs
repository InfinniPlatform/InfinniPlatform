namespace InfinniPlatform.Hosting
{
    /// <summary>
    ///     Сервис хостинга.
    /// </summary>
    public interface IHostingService
    {
        /// <summary>
        ///     Контекст подсистемы хостинга.
        /// </summary>
        IHostingContext Context { get; }

        /// <summary>
        ///     Запустить хостинг.
        /// </summary>
        void Start();

        /// <summary>
        ///     Остановить хостинг.
        /// </summary>
        void Stop();
    }
}