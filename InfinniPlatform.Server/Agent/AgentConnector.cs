using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Server.Settings;

namespace InfinniPlatform.Server.Agent
{
    public class AgentConnector : IAgentConnector
    {
        private const string InstallVerb = "install";
        private const string UninstallVerb = "uninstall";
        private const string InitVerb = "init";
        private const string StartVerb = "start";
        private const string StopVerb = "stop";
        private const string RestartVerb = "restart";
        private const string AppsInfoVerb = "appsInfo";

        public AgentConnector(ServerSettings serverSettings)
        {
            _httpClient = new HttpClient();
            _serverSettings = serverSettings;
        }

        private readonly HttpClient _httpClient;
        private readonly ServerSettings _serverSettings;

        public Task<object> GetAgentsInfo()
        {
            return Task.FromResult<object>(_serverSettings.AgentsInfo);
        }

        public async Task<object> InstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecutePostRequest(InstallVerb, agentAddress, agentPort, arguments);
        }

        public async Task<object> UninstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecutePostRequest(UninstallVerb, agentAddress, agentPort, arguments);
        }

        public async Task<object> InitApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecutePostRequest(InitVerb, agentAddress, agentPort, arguments);
        }

        public async Task<object> StartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecutePostRequest(StartVerb, agentAddress, agentPort, arguments);
        }

        public async Task<object> StopApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecutePostRequest(StopVerb, agentAddress, agentPort, arguments);
        }

        public async Task<object> RestartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecutePostRequest(RestartVerb, agentAddress, agentPort, arguments);
        }

        public async Task<object> GetAppsInfo(string agentAddress, int agentPort)
        {
            return await ExecuteGetRequest(AppsInfoVerb, agentAddress, agentPort);
        }

        private async Task<object> ExecuteGetRequest(string command, string agentAddress, int agentPort)
        {
            var uriString = $"http://{agentAddress}:{agentPort}/node/{command}";
            var response = await _httpClient.GetAsync(uriString);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        private async Task<object> ExecutePostRequest(string command, string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent)
        {
            var uriString = $"http://{agentAddress}:{agentPort}/node/{command}";
            var response = await _httpClient.PostAsync(new Uri(uriString), new FormUrlEncodedContent(formContent));
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}