using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Server.Settings;

namespace InfinniPlatform.Server.Agent
{
    public class AgentConnector : IAgentConnector
    {
        private const string InstallPath = "install";
        private const string UninstallPath = "uninstall";
        private const string InitPath = "init";
        private const string StartPath = "start";
        private const string StopPath = "stop";
        private const string RestartPath = "restart";
        private const string AppsInfoPath = "appsInfo";
        private const string ConfigPath = "config";
        private const string VariablesPath = "variables";
        private const string VariablePath = "variable";

        public AgentConnector(ServerSettings serverSettings)
        {
            _httpClient = new HttpClient();
            _serverSettings = serverSettings;
        }

        private readonly HttpClient _httpClient;
        private readonly ServerSettings _serverSettings;

        public AgentInfo[] GetAgentsInfo()
        {
            return _serverSettings.AgentsInfo;
        }

        public async Task<object> InstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecutePostRequest(InstallPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> UninstallApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecutePostRequest(UninstallPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> InitApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecutePostRequest(InitPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> StartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecutePostRequest(StartPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> StopApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecutePostRequest(StopPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> RestartApp(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecutePostRequest(RestartPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> GetAppsInfo(string agentAddress, int agentPort)
        {
            return await ExecuteGetRequest(AppsInfoPath, agentAddress, agentPort);
        }

        public async Task<object> GetAppsInfo(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecuteGetRequest(AppsInfoPath, agentAddress, agentPort);
        }

        public async Task<object> GetConfigurationFile(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecuteGetRequest(ConfigPath, agentAddress, agentPort);
        }

        public async Task<object> SetConfigurationFile(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecutePostRequest(ConfigPath, agentAddress, agentPort, arguments);
        }

        public async Task<object> GetVariables(string agentAddress, int agentPort)
        {
            return await ExecuteGetRequest(VariablesPath, agentAddress, agentPort);
        }

        public async Task<object> GetVariable(string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> arguments)
        {
            return await ExecuteGetRequest(VariablePath, agentAddress, agentPort);
        }

        private async Task<object> ExecuteGetRequest(string path, string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> query = null)
        {
            var uriString = $"http://{agentAddress}:{agentPort}/node/{path}{ToQuery(query)}";
            var response = await _httpClient.GetAsync(uriString);
            var content = await response.Content.ReadAsStringAsync();

            //var executeGetRequest = JsonObjectSerializer.Formated.Deserialize(content, typeof(object[]));

            return content;
        }

        private async Task<object> ExecutePostRequest(string path, string agentAddress, int agentPort, IEnumerable<KeyValuePair<string, string>> formContent)
        {
            var uriString = $"http://{agentAddress}:{agentPort}/node/{path}";
            var response = await _httpClient.PostAsync(new Uri(uriString), new FormUrlEncodedContent(formContent));
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }

        private static string ToQuery(IEnumerable<KeyValuePair<string, string>> queryContent)
        {
            if (queryContent == null)
            {
                return null;
            }

            var query = "?";

            foreach (var pair in queryContent)
            {
                query += $"{pair.Key}={pair.Value}&";
            }

            return query.TrimEnd('&');
        }
    }
}