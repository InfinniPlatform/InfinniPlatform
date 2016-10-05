using System.Threading.Tasks;

namespace InfinniPlatform.Agent.InfinniNode
{
    /// <summary>
    /// Интерфейс взаимодействия с утилитой Infinni.Node.
    /// </summary>
    public interface IConnector
    {
        /// <summary>
        /// Вызывает команду установки приложения.
        /// </summary>
        /// <param name="appName">Имя приложения.</param>
        Task<object> InstallApp(string appName);

        /// <summary>
        /// Вызывает команду удаления приложения.
        /// </summary>
        /// <param name="appName">Имя приложения.</param>
        Task<object> UninstallApp(string appName);

        /// <summary>
        /// Вызывает команду запуска приложения.
        /// </summary>
        /// <param name="appName">Имя приложения.</param>
        Task<object> StartApp(string appName);

        /// <summary>
        /// Вызывает команду остановки приложения.
        /// </summary>
        /// <param name="appName">Имя приложения.</param>
        Task<object> StopApp(string appName);

        /// <summary>
        /// Возвращает информацию о приложениях успановленных на машине.
        /// </summary>
        Task<object[]> GetInstalledAppsInfo();
    }
}