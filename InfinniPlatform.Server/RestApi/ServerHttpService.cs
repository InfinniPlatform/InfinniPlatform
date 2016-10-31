using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.PushNotification;
using InfinniPlatform.Server.Agent;
using InfinniPlatform.Server.Settings;

namespace InfinniPlatform.Server.RestApi
{
    /// <summary>
    /// Сервис взаимодействия с приложением Infinni.Agent.
    /// </summary>
    public class ServerHttpService : IHttpService
    {
        private const string NotifyMessageType = "WorkLog";

        public ServerHttpService(IAgentHttpClient agentHttpClient,
                                 ILog log,
                                 INodeOutputParser nodeOutputParser,
                                 IPushNotificationService notifyService,
                                 ServerSettings serverSettings
        )
        {
            _agentHttpClient = agentHttpClient;
            _log = log;
            _nodeOutputParser = nodeOutputParser;
            _notifyService = notifyService;
            _serverSettings = serverSettings;
        }

        private readonly IAgentHttpClient _agentHttpClient;
        private readonly ILog _log;
        private readonly INodeOutputParser _nodeOutputParser;
        private readonly IPushNotificationService _notifyService;
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

            var args = new DynamicWrapper
                       {
                           { "AppName", ParseString(request.Form.AppName) },
                           { "Version", ParseString(request.Form.Version) },
                           { "Instance", ParseString(request.Form.Instance) },
                           { "Source", ParseString(request.Form.Source) },
                           { "AllowPrerelease", (bool?)request.Form.AllowPrerelease }
                       };

            await _notifyService.NotifyAll(NotifyMessageType, $"Installing {args["AppName"]}...");

            var serviceResult = await _agentHttpClient.Post<ServiceResult<ProcessResult>>("install", address, port, args);

            await _notifyService.NotifyAll(NotifyMessageType, $"Installing {args["AppName"]} complete.");

            return serviceResult;
        }

        private async Task<object> UninstallApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var args = new DynamicWrapper
                       {
                           { "AppName", ParseString(request.Form.AppName) },
                           { "Version", ParseString(request.Form.Version) },
                           { "Instance", ParseString(request.Form.Instance) }
                       };

            await _notifyService.NotifyAll(NotifyMessageType, $"Uninstall {args["AppName"]}...");

            var serviceResult = await _agentHttpClient.Post<ServiceResult<ProcessResult>>("uninstall", address, port, args);

            await _notifyService.NotifyAll(NotifyMessageType, $"Uninstall {args["AppName"]} complete.");

            return serviceResult;
        }

        private async Task<object> InitApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var args = new DynamicWrapper
                       {
                           { "AppName", ParseString(request.Form.AppName) },
                           { "Version", ParseString(request.Form.Version) },
                           { "Instance", ParseString(request.Form.Instance) },
                           { "Timeout", ParseInt(request.Form.Timeout) }
                       };

            await _notifyService.NotifyAll(NotifyMessageType, $"Initializing {args["AppName"]}...");

            var serviceResult = await _agentHttpClient.Post<ServiceResult<ProcessResult>>("init", address, port, args);

            await _notifyService.NotifyAll(NotifyMessageType, $"Initializing {args["AppName"]} complete.");

            return serviceResult;
        }

        private async Task<object> StartApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var args = new DynamicWrapper
                       {
                           { "AppName", ParseString(request.Form.AppName) },
                           { "Version", ParseString(request.Form.Version) },
                           { "Instance", ParseString(request.Form.Instance) },
                           { "Timeout", ParseInt(request.Form.Timeout) }
                       };

            await _notifyService.NotifyAll(NotifyMessageType, $"Starting {args["AppName"]}...");

            var serviceResult = await _agentHttpClient.Post<ServiceResult<ProcessResult>>("start", address, port, args);

            await _notifyService.NotifyAll(NotifyMessageType, $"Starting {args["AppName"]} comleted!");

            return serviceResult;
        }

        private async Task<object> StopApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var args = new DynamicWrapper
                       {
                           { "AppName", ParseString(request.Form.AppName) },
                           { "Version", ParseString(request.Form.Version) },
                           { "Instance", ParseString(request.Form.Instance) },
                           { "Timeout", ParseInt(request.Form.Timeout) }
                       };

            await _notifyService.NotifyAll(NotifyMessageType, $"Stopping {args["AppName"]}...");

            var serviceResult = await _agentHttpClient.Post<ServiceResult<ProcessResult>>("stop", address, port, args);

            await _notifyService.NotifyAll(NotifyMessageType, $"Stopping {args["AppName"]} comleted!");

            return serviceResult;
        }

        private async Task<object> RestartApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var args = new DynamicWrapper
                       {
                           { "AppName", ParseString(request.Form.AppName) },
                           { "Version", ParseString(request.Form.Version) },
                           { "Instance", ParseString(request.Form.Instance) },
                           { "Timeout", ParseInt(request.Form.Timeout) }
                       };

            await _notifyService.NotifyAll(NotifyMessageType, $"Restarting {args["AppName"]}...");

            var serviceResult = await _agentHttpClient.Post<ServiceResult<ProcessResult>>("restart", address, port, args);

            await _notifyService.NotifyAll(NotifyMessageType, $"Restarting {args["AppName"]}...");

            return serviceResult;
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

            var appsInfo = _nodeOutputParser.FormatAppsInfoOutput(serviceResult);

            return appsInfo;
        }

        private async Task<object> GetEnvironmentVariables(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var environmentVariables = await _agentHttpClient.Get<ServiceResult<object>>("variables", address, port);

            return environmentVariables;
        }

        private async Task<object> GetEnvironmentVariable(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var args = new DynamicWrapper
                       {
                           { "Name", ParseString(request.Query.Name) }
                       };

            var environmentVariable = await _agentHttpClient.Get<ServiceResult<object>>("variable", address, port, args);

            return environmentVariable;
        }

        private Task<object> LogBeat(IHttpRequest request)
        {
            string s = request.Form.Message;

            _log.Info(s, () => new Dictionary<string, object>
                               {
                                   { "Name", ParseString(request.Form.Name) },
                                   { "InstanceId", ParseString(request.Form.InstanceId) }
                               });

            var serviceResult = new ServiceResult<object>
                                {
                                    Success = true
                                };

            return Task.FromResult<object>(serviceResult);
        }

        private static int? ParseInt(dynamic value)
        {
            return string.IsNullOrEmpty(value)
                       ? null
                       : int.Parse(value);
        }

        private static string ParseString(dynamic value)
        {
            return string.IsNullOrEmpty(value) || (value == "null")
                       ? null
                       : (string)value;
        }
    }
}