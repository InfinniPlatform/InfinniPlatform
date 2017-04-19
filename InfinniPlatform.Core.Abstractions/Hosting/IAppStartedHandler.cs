namespace InfinniPlatform.Core.Hosting
{
    /// <summary>
    /// Обработчик события запуска приложения.
    /// </summary>
    public interface IAppStartedHandler
    {
        /// <summary>
        /// Вызывается после запуска приложения.
        /// </summary>
        void Handle();
    }
}