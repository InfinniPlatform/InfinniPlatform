using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinniPlatform.Server.Agent
{
    public interface IAgentConnector
    {
        Task<object> GetAgentsStatus();

        Task<object> InstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent);

        Task<object> UninstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent);

        Task<object> InitApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent);

        Task<object> StartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent);

        Task<object> StopApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent);

        Task<object> RestartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent);

        Task<object> GetAppsInfo(string agentAddress, int agentPort);
    }
}