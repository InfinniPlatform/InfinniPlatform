namespace InfinniPlatform.Core.Hosting
{
    /// <summary>
    /// Обработчик события остановки приложения.
    /// </summary>
    public interface IAppStoppedHandler
    {
        /// <summary>
        /// Вызывается после остановки приложения.
        /// </summary>
        void Handle();
    }
}