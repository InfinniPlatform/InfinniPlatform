using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinniPlatform.Server.Agent
{
    /// <summary>
    /// Интерфейс взаимодействия с агентами.
    /// </summary>
    public interface IAgentConnector
    {
        /// <summary>
        /// Возвращает информацию о подключенных агентах.
        /// </summary>
        Task<object> GetAgentsInfo();

        /// <summary>
        /// Отправляет команду установки приложения.
        /// </summary>
        /// <param name="agentAddress">Адрес агента.</param>
        /// <param name="agentPort">Порт агента.</param>
        /// <param name="arguments">Аргументы команды.</param>
        Task<object> InstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        /// <summary>
        /// Отправляет команду удаления приложения.
        /// </summary>
        /// <param name="agentAddress">Адрес агента.</param>
        /// <param name="agentPort">Порт агента.</param>
        /// <param name="arguments">Аргументы команды.</param>
        Task<object> UninstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        /// <summary>
        /// Отправляет команду инициализации приложения.
        /// </summary>
        /// <param name="agentAddress">Адрес агента.</param>
        /// <param name="agentPort">Порт агента.</param>
        /// <param name="arguments">Аргументы команды.</param>
        Task<object> InitApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        /// <summary>
        /// Отправляет команду запуска приложения.
        /// </summary>
        /// <param name="agentAddress">Адрес агента.</param>
        /// <param name="agentPort">Порт агента.</param>
        /// <param name="arguments">Аргументы команды.</param>
        Task<object> StartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        /// <summary>
        /// Отправляет команду остановки приложения.
        /// </summary>
        /// <param name="agentAddress">Адрес агента.</param>
        /// <param name="agentPort">Порт агента.</param>
        /// <param name="arguments">Аргументы команды.</param>
        Task<object> StopApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        /// <summary>
        /// Отправляет команду перезапуска приложения.
        /// </summary>
        /// <param name="agentAddress">Адрес агента.</param>
        /// <param name="agentPort">Порт агента.</param>
        /// <param name="arguments">Аргументы команды.</param>
        Task<object> RestartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        /// <summary>
        /// Отправляет команду получения информации о приложениях.
        /// </summary>
        /// <param name="agentAddress">Адрес агента.</param>
        /// <param name="agentPort">Порт агента.</param>
        Task<object> GetAppsInfo(string agentAddress, int agentPort);
    }
}