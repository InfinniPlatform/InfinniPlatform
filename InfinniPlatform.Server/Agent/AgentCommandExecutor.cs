using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Server.Settings;

namespace InfinniPlatform.Server.Agent
{
    public class AgentCommandExecutor : IAgentCommandExecutor
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
        private const string OutputInfoRegex = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2},\d{3}\s\[PID\s\d{1,10}\]\sINFO\s+-\s+";
        private const string OutputErrorRegex = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2},\d{3}\s\[PID\s\d{1,10}\]\sERROR\s+-\s+";

        public AgentCommandExecutor(ServerSettings serverSettings, IJsonObjectSerializer serializer)
        {
            _httpClient = new HttpClient();
            _serverSettings = serverSettings;
            _serializer = serializer;
        }

        private readonly HttpClient _httpClient;
        private readonly IJsonObjectSerializer _serializer;
        private readonly ServerSettings _serverSettings;

        public object GetAgentsInfo()
        {
            return new ServiceResult<object>
                   {
                       Success = true,
                       Result = new DynamicWrapper { { "Agents", _serverSettings.AgentsInfo } }
                   };
        }

        public async Task<object> InstallApp(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            var processResult = await ExecutePostRequest<ProcessResult>(InstallPath, agentAddress, agentPort, arguments);
            return processResult;
        }

        public async Task<object> UninstallApp(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            var processResult = await ExecutePostRequest<ProcessResult>(UninstallPath, agentAddress, agentPort, arguments);
            return processResult;
        }

        public async Task<object> InitApp(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            var processResult = await ExecutePostRequest<ProcessResult>(InitPath, agentAddress, agentPort, arguments);
            return processResult;
        }

        public async Task<object> StartApp(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            var processResult = await ExecutePostRequest<ProcessResult>(StartPath, agentAddress, agentPort, arguments);
            return processResult;
        }

        public async Task<object> StopApp(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            var processResult = await ExecutePostRequest<ProcessResult>(StopPath, agentAddress, agentPort, arguments);
            return processResult;
        }

        public async Task<object> RestartApp(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            var processResult = await ExecutePostRequest<ProcessResult>(RestartPath, agentAddress, agentPort, arguments);
            return processResult;
        }

        public async Task<object> GetAppsInfo(string agentAddress, int agentPort)
        {
            var serviceResult = await ExecuteGetRequest<ProcessResult>(AppsInfoPath, agentAddress, agentPort);

            var processResult = serviceResult.Result;

            var appsInfoJson = Regex.Replace(processResult.Output, OutputInfoRegex, string.Empty, RegexOptions.Multiline, TimeSpan.FromMinutes(1))
                                    .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                                    .FirstOrDefault(s => s.StartsWith("[{"));

            processResult.FormatedOutput = _serializer.Deserialize<object[]>(appsInfoJson);

            serviceResult.Result = processResult;

            return serviceResult;
        }

        public async Task<object> GetConfigurationFile(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            var configurationFile = await ExecuteGetRequest<object>(ConfigPath, agentAddress, agentPort, arguments);
            return configurationFile;
        }

        public async Task<object> SetConfigurationFile(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            var configurationFile = await ExecutePostRequest<object>(ConfigPath, agentAddress, agentPort, arguments);
            return configurationFile;
        }

        public async Task<object> GetVariables(string agentAddress, int agentPort)
        {
            var processResult = await ExecuteGetRequest<object>(VariablesPath, agentAddress, agentPort);
            return processResult;
        }

        public async Task<object> GetVariable(string agentAddress, int agentPort, DynamicWrapper arguments)
        {
            var processResult = await ExecuteGetRequest<object>(VariablePath, agentAddress, agentPort, arguments);
            return processResult;
        }

        private async Task<ServiceResult<T>> ExecuteGetRequest<T>(string path, string agentAddress, int agentPort, DynamicWrapper queryContent = null)
        {
            var uriString = $"http://{agentAddress}:{agentPort}/agent/{path}{ToQuery(queryContent)}";

            var response = await _httpClient.GetAsync(uriString);

            var content = await response.Content.ReadAsStreamAsync();

            var processResult = _serializer.Deserialize<T>(content);

            var serviceResult = new ServiceResult<T>
                                {
                                    Success = true,
                                    Result = processResult
                                };

            return serviceResult;
        }

        private async Task<ServiceResult<T>> ExecutePostRequest<T>(string path, string agentAddress, int agentPort, DynamicWrapper formContent)
        {
            var uriString = $"http://{agentAddress}:{agentPort}/agent/{path}";

            var convertToString = _serializer.ConvertToString(formContent);
            var requestContent = new StringContent(convertToString, _serializer.Encoding, HttpConstants.JsonContentType);

            var response = await _httpClient.PostAsync(new Uri(uriString), requestContent);

            var content = await response.Content.ReadAsStringAsync();
            var processResult = _serializer.Deserialize<T>(content);

            var serviceResult = new ServiceResult<T>
                                {
                                    Success = true,
                                    Result = processResult
                                };

            return serviceResult;
        }

        private static string ToQuery(DynamicWrapper queryContent)
        {
            if (queryContent == null)
            {
                return null;
            }

            var query = "?";

            foreach (var pair in queryContent.ToDictionary())
            {
                query += $"{pair.Key}={pair.Value}&";
            }

            return query.TrimEnd('&');
        }


        public struct ProcessResult
        {
            public bool Completed;
            public int? ExitCode;
            public object FormatedOutput;
            public string Output;
        }
    }
}