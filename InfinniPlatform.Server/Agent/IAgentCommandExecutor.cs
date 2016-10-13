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
        /// Отправляет команду установки приложения.
        /// </summary>
        Task<object> InstallApp(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// Отправляет команду удаления приложения.
        /// </summary>
        Task<object> UninstallApp(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// Отправляет команду инициализации приложения.
        /// </summary>
        Task<object> InitApp(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// Отправляет команду запуска приложения.
        /// </summary>
        Task<object> StartApp(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// Отправляет команду остановки приложения.
        /// </summary>
        Task<object> StopApp(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// 
        /// </summary>
        Task<object> RestartApp(string agentAddress, int agentPort, DynamicWrapper arguments);

        /// <summary>
        /// Отправляет команду установки приложения.
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
    }
}