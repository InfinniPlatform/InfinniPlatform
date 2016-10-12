using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Server.Settings;

namespace InfinniPlatform.Server.Agent
{
    /// <summary>
    /// Интерфейс взаимодействия с агентами.
    /// </summary>
    public interface IAgentConnector
    {
        AgentInfo[] GetAgentsInfo();

        Task<object> InstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        Task<object> UninstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        Task<object> InitApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        Task<object> StartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        Task<object> StopApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        Task<object> RestartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        Task<object> GetAppsInfo(string agentAddress, int agentPort);

        Task<object> GetAppsInfo(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        Task<object> GetConfigurationFile(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        Task<object> SetConfigurationFile(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);

        Task<object> GetVariables(string agentAddress, int agentPort);

        Task<object> GetVariable(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments);
    }
}