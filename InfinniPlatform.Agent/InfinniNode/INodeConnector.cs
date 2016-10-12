using System.Threading.Tasks;

namespace InfinniPlatform.Agent.InfinniNode
{
    /// <summary>
    /// Интерфейс взаимодействия с утилитой Infinni.Node.
    /// </summary>
    public interface INodeConnector
    {
        /// <summary>
        /// Вызывает команду установки приложения.
        /// </summary>
        /// <param name="appName">Имя приложения.</param>
        /// <param name="version">Версия приложения.</param>
        /// <param name="instance">Имя экземпляра приложения.</param>
        /// <param name="source">Список источников приложений.</param>
        /// <param name="allowPrerelease">Разрешает установку предрелизных версий приложений.</param>
        Task<ProcessHelper.ProcessResult> InstallApp(string appName, string version = null, string instance = null, string source = null, bool? allowPrerelease = null);

        /// <summary>
        /// Вызывает команду удаления приложения.
        /// </summary>
        /// <param name="appName">Имя приложения.</param>
        /// <param name="version">Версия приложения.</param>
        /// <param name="instance">Имя экземпляра приложения.</param>
        Task<ProcessHelper.ProcessResult> UninstallApp(string appName = null, string version = null, string instance = null);

        /// <summary>
        /// Вызывает команду инициализации приложения.
        /// </summary>
        /// <param name="appName">Имя приложения.</param>
        /// <param name="version">Версия приложения.</param>
        /// <param name="instance">Имя экземпляра приложения.</param>
        /// <param name="timeout">Таймаут выполнения команды.</param>
        Task<ProcessHelper.ProcessResult> InitApp(string appName = null, string version = null, string instance = null, int? timeout = null);

        /// <summary>
        /// Вызывает команду запуска приложения.
        /// </summary>
        /// <param name="appName">Имя приложения.</param>
        /// <param name="version">Версия приложения.</param>
        /// <param name="instance">Имя экземпляра приложения.</param>
        /// <param name="timeout">Таймаут выполнения команды.</param>
        Task<ProcessHelper.ProcessResult> StartApp(string appName = null, string version = null, string instance = null, int? timeout = null);

        /// <summary>
        /// Вызывает команду остановки приложения.
        /// </summary>
        /// <param name="appName">Имя приложения.</param>
        /// <param name="version">Версия приложения.</param>
        /// <param name="instance">Имя экземпляра приложения.</param>
        /// <param name="timeout">Таймаут выполнения команды.</param>
        Task<ProcessHelper.ProcessResult> StopApp(string appName = null, string version = null, string instance = null, int? timeout = null);

        /// <summary>
        /// Вызывает команду перезапуска приложения.
        /// </summary>
        /// <param name="appName">Имя приложения.</param>
        /// <param name="version">Версия приложения.</param>
        /// <param name="instance">Имя экземпляра приложения.</param>
        /// <param name="timeout">Таймаут выполнения команды.</param>
        Task<ProcessHelper.ProcessResult> RestartApp(string appName = null, string version = null, string instance = null, int? timeout = null);

        /// <summary>
        /// Возвращает информацию об установленных приложениях.
        /// </summary>
        /// <returns></returns>
        Task<ProcessHelper.ProcessResult> GetInstalledAppsInfo();
    }
}