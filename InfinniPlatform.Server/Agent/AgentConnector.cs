using System.Net.Http;

using InfinniPlatform.Server.Settings;

namespace InfinniPlatform.Server.Agent
{
    public class AgentConnector : IAgentConnector
    {
        public AgentConnector(ServerSettings serverSettings)
        {
            _serverSettings = serverSettings;
            _httpClient = new HttpClient();
        }

        private readonly ServerSettings _serverSettings;
        private HttpClient _httpClient;

        public AgentInfo[] GetAgentsStatus()
        {
            return _serverSettings.AgentsInfo;
        }
    }


    public interface IAgentConnector
    {
        /// <summary>
        /// Возвращает информацию об агентах.
        /// </summary>
        /// <returns></returns>
        AgentInfo[] GetAgentsStatus();
    }
}