using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Server.Agent;

namespace InfinniPlatform.Server.RestApi
{
    /// <summary>
    /// Сервис взаимодействия с приложением Infinni.Agent.
    /// </summary>
    public class ServerHttpService : IHttpService
    {
        public ServerHttpService(IAgentCommandExecutor agentCommandExecutor,
                                 ILog log)
        {
            _agentCommandExecutor = agentCommandExecutor;
            _log = log;
        }

        private readonly IAgentCommandExecutor _agentCommandExecutor;
        private readonly ILog _log;

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

            builder.Get["/config"] = GetConfigurationFile;
            builder.Post["/config"] = SetConfigurationFile;

            builder.Get["/variables"] = GetEnvironmentVariables;
            builder.Get["/variable"] = GetEnvironmentVariable;

            builder.Post["/heartbeat"] = LogBeat;
        }

        private Task<object> GetAgentsStatus(IHttpRequest httpRequest)
        {
            // TODO return status for agents.
            return Task.FromResult(_agentCommandExecutor.GetAgentsInfo());
        }

        private async Task<object> InstallApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var wrapper = new DynamicWrapper
                          {
                              { "AppName", (string)request.Form.AppName },
                              { "Version", (string)request.Form.Version },
                              { "Instance", (string)request.Form.Instance },
                              { "Source", (string)request.Form.Source },
                              { "AllowPrerelease", (bool?)request.Form.AllowPrerelease }
                          };

            return await _agentCommandExecutor.InstallApp(address, port, wrapper);
        }

        private async Task<object> UninstallApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var wrapper = new DynamicWrapper
                          {
                              { "AppName", (string)request.Form.AppName },
                              { "Version", (string)request.Form.Version },
                              { "Instance", (string)request.Form.Instance }
                          };

            return await _agentCommandExecutor.UninstallApp(address, port, wrapper);
        }

        private async Task<object> InitApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var wrapper = new DynamicWrapper
                          {
                              { "AppName", (string)request.Form.AppName },
                              { "Version", (string)request.Form.Version },
                              { "Instance", (string)request.Form.Instance },
                              { "Timeout", ParseTimeout(request) }
                          };

            return await _agentCommandExecutor.InitApp(address, port, wrapper);
        }

        private async Task<object> StartApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var wrapper = new DynamicWrapper
                          {
                              { "AppName", (string)request.Form.AppName },
                              { "Version", (string)request.Form.Version },
                              { "Instance", (string)request.Form.Instance },
                              { "Timeout", ParseTimeout(request) }
                          };

            return await _agentCommandExecutor.StartApp(address, port, wrapper);
        }

        private static dynamic ParseTimeout(IHttpRequest request)
        {
            return string.IsNullOrEmpty(request.Form.Timeout)
                       ? null
                       : int.Parse(request.Form.Timeout);
        }

        private async Task<object> StopApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var wrapper = new DynamicWrapper
                          {
                              { "AppName", (string)request.Form.AppName },
                              { "Version", (string)request.Form.Version },
                              { "Instance", (string)request.Form.Instance },
                              { "Timeout", ParseTimeout(request) }
                          };

            return await _agentCommandExecutor.StopApp(address, port, wrapper);
        }

        private async Task<object> RestartApp(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var wrapper = new DynamicWrapper
                          {
                              { "AppName", (string)request.Form.AppName },
                              { "Version", (string)request.Form.Version },
                              { "Instance", (string)request.Form.Instance },
                              { "Timeout", ParseTimeout(request) }
                          };

            return await _agentCommandExecutor.RestartApp(address, port, wrapper);
        }

        private async Task<object> GetAppsInfo(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var serviceResult = await _agentCommandExecutor.GetAppsInfo(address, port);

            return serviceResult;
        }

        private async Task<object> GetConfigurationFile(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var wrapper = new DynamicWrapper
                          {
                              { "AppFullName", (string)request.Query.AppFullName },
                              { "FileName", (string)request.Query.FileName }
                          };

            return await _agentCommandExecutor.GetConfigurationFile(address, port, wrapper);
        }

        private async Task<object> SetConfigurationFile(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var wrapper = new DynamicWrapper
                          {
                              { "AppFullName", (string)request.Query.AppFullName },
                              { "FileName", (string)request.Query.FileName },
                              { "Config", (string)request.Form.Config }
                          };

            return await _agentCommandExecutor.SetConfigurationFile(address, port, wrapper);
        }

        private async Task<object> GetEnvironmentVariables(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            return await _agentCommandExecutor.GetVariables(address, port);
        }

        private async Task<object> GetEnvironmentVariable(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var wrapper = new DynamicWrapper
                          {
                              { "Name", (string)request.Query.Name }
                          };

            return await _agentCommandExecutor.GetVariable(address, port, wrapper);
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
    }
}