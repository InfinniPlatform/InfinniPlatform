using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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

        private readonly HttpClient _httpClient;

        private readonly ServerSettings _serverSettings;

        public Task<object> GetAgentsStatus()
        {
            return Task.FromResult<object>(_serverSettings.AgentsInfo);
        }

        public async Task<object> InstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent)
        {
            return await ExecutePostRequest("install", agentAddress, agentPort, formContent);
        }

        public async Task<object> UninstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent)
        {
            return await ExecutePostRequest("uninstall", agentAddress, agentPort, formContent);
        }

        public async Task<object> InitApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent)
        {
            return await ExecutePostRequest("init", agentAddress, agentPort, formContent);
        }

        public async Task<object> StartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent)
        {
            return await ExecutePostRequest("start", agentAddress, agentPort, formContent);
        }

        public async Task<object> StopApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent)
        {
            return await ExecutePostRequest("stop", agentAddress, agentPort, formContent);
        }

        public async Task<object> RestartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent)
        {
            return await ExecutePostRequest("restart", agentAddress, agentPort, formContent);
        }

        private async Task<object> ExecutePostRequest(string command, string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent)
        {
            var uriString = $"http://{agentAddress}:{agentPort}/node/{command}";
            var response = await _httpClient.PostAsync(new Uri(uriString), new FormUrlEncodedContent(formContent));
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }


    public interface IAgentConnector
    {
        Task<object> GetAgentsStatus();

        Task<object> InstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent);

        Task<object> UninstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent);

        Task<object> InitApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent);

        Task<object> StartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent);

        Task<object> StopApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent);

        Task<object> RestartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent);
    }
}