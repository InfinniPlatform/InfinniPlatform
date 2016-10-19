using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Server.Agent
{
    /// <summary>
    /// Интерфейс взаимодействия с агентами.
    /// </summary>
    public interface IAgentCommandExecutor
    {
        /// <summary>
        /// Возвращает информацию о подключенных агентах.
        /// </summary>
        object GetAgentsInfo();

        /// <summary>
        /// Выполняет установку приложения.
        /// </summary>
        Task<object> InstallApp(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// Выполняет удаление приложения.
        /// </summary>
        Task<object> UninstallApp(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// Выполняет инициализацию приложения.
        /// </summary>
        Task<object> InitApp(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// Выполняет запуск приложения.
        /// </summary>
        Task<object> StartApp(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// Выполняет остановку приложения.
        /// </summary>
        Task<object> StopApp(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// Выполняет перезапуск приложения.
        /// </summary>
        Task<object> RestartApp(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// Возвращает информацию о приложениях.
        /// </summary>
        Task<object> GetAppsInfo(string agentAddress, int agentPort);

        /// <summary>
        /// Возвращает содержимое конфигурационного файла.
        /// </summary>
        Task<object> GetConfigurationFile(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// Возвращает содержимое конфигурационного файла.
        /// </summary>
        Task<object> SetConfigurationFile(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// Возвращает переменные окружения.
        /// </summary>
        Task<object> GetVariables(string agentAddress, int agentPort);

        /// <summary>
        /// Возвращает переменную окружения по имени.
        /// </summary>
        Task<object> GetVariable(string agentAddress, int agentPort, DynamicWrapper arguments);

        Task<object> GetAppLogFile(string agentAddress, int agentPort, DynamicWrapper arguments);

        Task<object> GetPerfLogFile(string agentAddress, int agentPort, DynamicWrapper arguments);

        Task<object> GetNodeLogFile(string agentAddress, int agentPort, DynamicWrapper arguments);
    }
}