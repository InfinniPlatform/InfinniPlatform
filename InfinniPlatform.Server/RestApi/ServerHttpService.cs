using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Server.Agent;
using InfinniPlatform.Server.Settings;

namespace InfinniPlatform.Server.RestApi
{
    /// <summary>
    /// Сервис взаимодействия с приложением Infinni.Agent.
    /// </summary>
    public class ServerHttpService : IHttpService
    {
        public ServerHttpService(IAgentHttpClient agentHttpClient,
                                 INodeOutputParser nodeOutputParser,
                                 ServerSettings serverSettings,
                                 ILog log)
        {
            _agentHttpClient = agentHttpClient;
            _nodeOutputParser = nodeOutputParser;
            _serverSettings = serverSettings;
            _log = log;
        }

        private readonly IAgentHttpClient _agentHttpClient;
        private readonly ILog _log;
        private readonly INodeOutputParser _nodeOutputParser;
        private readonly ServerSettings _serverSettings;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "server";

            builder.Get["/agents"] = GetAgentsStatus;

            builder.Post["/install"] = InstallApp;
            builder.Post["/uninstall"] = UninstallApp;

            builder.Post["/init"] = InitApp;
            builder.Post["/start"] = StartApp;
            builder.Post["/stop"] = StopApp;
            builder.Post["/restart"] = RestartApp;

            builder.Get["/appsInfo"] = GetAppsInfo;

            builder.Get["/variables"] = GetEnvironmentVariables;
            builder.Get["/variable"] = GetEnvironmentVariable;

            builder.Post["/heartbeat"] = LogBeat;
        }

        private Task<object> GetAgentsStatus(IHttpRequest httpRequest)
        {
            var agents = new DynamicWrapper { { "Agents", _serverSettings.AgentsInfo } };

            return Task.FromResult<object>(new ServiceResult<DynamicWrapper> { Success = true, Result = agents });
        }

        private async Task<object> InstallApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppName", (string)request.Form.AppName },
                                { "Version", (string)request.Form.Version },
                                { "Instance", (string)request.Form.Instance },
                                { "Source", (string)request.Form.Source },
                                { "AllowPrerelease", (bool?)request.Form.AllowPrerelease }
                            };

            return await _agentHttpClient.Post<ServiceResult<ProcessResult>>("install", address, port, arguments);
        }

        private async Task<object> UninstallApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppName", (string)request.Form.AppName },
                                { "Version", (string)request.Form.Version },
                                { "Instance", (string)request.Form.Instance }
                            };

            return await _agentHttpClient.Post<ServiceResult<ProcessResult>>("uninstall", address, port, arguments);
        }

        private async Task<object> InitApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppName", (string)request.Form.AppName },
                                { "Version", (string)request.Form.Version },
                                { "Instance", (string)request.Form.Instance },
                                { "Timeout", ParseTimeout(request) }
                            };

            return await _agentHttpClient.Post<ServiceResult<ProcessResult>>("init", address, port, arguments);
        }

        private async Task<object> StartApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppName", (string)request.Form.AppName },
                                { "Version", (string)request.Form.Version },
                                { "Instance", (string)request.Form.Instance },
                                { "Timeout", ParseTimeout(request) }
                            };

            return await _agentHttpClient.Post<ServiceResult<ProcessResult>>("start", address, port, arguments);
        }

        private async Task<object> StopApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppName", (string)request.Form.AppName },
                                { "Version", (string)request.Form.Version },
                                { "Instance", (string)request.Form.Instance },
                                { "Timeout", ParseTimeout(request) }
                            };

            return await _agentHttpClient.Post<ServiceResult<ProcessResult>>("stop", address, port, arguments);
        }

        private async Task<object> RestartApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "AppName", (string)request.Form.AppName },
                                { "Version", (string)request.Form.Version },
                                { "Instance", (string)request.Form.Instance },
                                { "Timeout", ParseTimeout(request) }
                            };

            return await _agentHttpClient.Post<ServiceResult<ProcessResult>>("restart", address, port, arguments);
        }

        private async Task<object> GetAppsInfo(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var serviceResult = await _agentHttpClient.Get<ServiceResult<ProcessResult>>("appsInfo", address, port);

            if (serviceResult == null)
            {
                return new ServiceResult<object> { Success = false, Error = "Agent response is empty." };
            }

            return _nodeOutputParser.FormatAppsStatusOutput(serviceResult);
        }

        private async Task<object> GetEnvironmentVariables(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            return await _agentHttpClient.Get<ServiceResult<object>>("variables", address, port);
        }

        private async Task<object> GetEnvironmentVariable(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "Name", (string)request.Query.Name }
                            };

            return await _agentHttpClient.Get<ServiceResult<object>>("variable", address, port, arguments);
        }

        private Task<object> LogBeat(IHttpRequest request)
        {
            string s = request.Form.Message;

            _log.Info(s, () => new Dictionary<string, object>
                               {
                                   { "Name", (string)request.Form.Name },
                                   { "InstanceId", (string)request.Form.InstanceId }
                               });

            return Task.FromResult<object>(new ServiceResult<object>
                                           {
                                               Success = true
                                           });
        }

        private static int? ParseTimeout(IHttpRequest request)
        {
            return string.IsNullOrEmpty(request.Form.Timeout)
                       ? null
                       : int.Parse(request.Form.Timeout);
        }
    }
}