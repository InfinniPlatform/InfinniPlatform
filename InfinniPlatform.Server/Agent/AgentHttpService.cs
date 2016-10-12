using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Server.Agent
{
    /// <summary>
    /// Сервис взаимодействия с утилитой Infinni.Node.
    /// </summary>
    public class AgentHttpService : IHttpService
    {
        public AgentHttpService(IAgentConnector agentConnector)
        {
            _agentConnector = agentConnector;
        }

        private readonly IAgentConnector _agentConnector;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "server";
            builder.Post["/agents"] = GetAgentsStatus;
            builder.Post["/installApp"] = InstallApp;
            builder.Post["/uninstallApp"] = UninstallApp;
            builder.Post["/initApp"] = InitApp;
            builder.Post["/startApp"] = StartApp;
            builder.Post["/stopApp"] = StopApp;
            builder.Post["/restartApp"] = RestartApp;
            builder.Post["/appsInfo"] = GetAppsInfo;
            builder.Post["/config"] = GetConfigurationFile;
            builder.Post["/config"] = SetConfigurationFile;
            builder.Post["/variables"] = GetEnvironmentVariables;
            builder.Post["/variable"] = GetEnvironmentVariable;
        }

        private Task<object> GetAgentsStatus(IHttpRequest httpRequest)
        {
            // TODO return status for agents.
            return Task.FromResult<object>(new JsonHttpResponse(_agentConnector.GetAgentsInfo()));
        }

        private async Task<object> InstallApp(IHttpRequest request)
        {
            string address = request.Form.Address;
            int port = request.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "AppName", (string)request.Form.AppName },
                                 { "Version", (string)request.Form.Version },
                                 { "Instance", (string)request.Form.Instance },
                                 { "Source", (string)request.Form.Source },
                                 { "AllowPrerelease", (string)request.Form.AllowPrerelease }
                             };

            return await _agentConnector.InstallApp(address, port, dictionary);
        }

        private async Task<object> UninstallApp(IHttpRequest request)
        {
            string address = request.Form.Address;
            int port = request.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "AppName", (string)request.Form.AppName },
                                 { "Version", (string)request.Form.Version },
                                 { "Instance", (string)request.Form.Instance }
                             };

            return await _agentConnector.UninstallApp(address, port, dictionary);
        }

        private async Task<object> InitApp(IHttpRequest request)
        {
            string address = request.Form.Address;
            int port = request.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "AppName", (string)request.Form.AppName },
                                 { "Version", (string)request.Form.Version },
                                 { "Instance", (string)request.Form.Instance },
                                 { "Timeout", (string)request.Form.Timeout }
                             };

            return await _agentConnector.InitApp(address, port, dictionary);
        }

        private async Task<object> StartApp(IHttpRequest request)
        {
            string address = request.Form.Address;
            int port = request.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "AppName", (string)request.Form.AppName },
                                 { "Version", (string)request.Form.Version },
                                 { "Instance", (string)request.Form.Instance },
                                 { "Timeout", (string)request.Form.Timeout }
                             };

            return await _agentConnector.StartApp(address, port, dictionary);
        }

        private async Task<object> StopApp(IHttpRequest request)
        {
            string address = request.Form.Address;
            int port = request.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "AppName", (string)request.Form.AppName },
                                 { "Version", (string)request.Form.Version },
                                 { "Instance", (string)request.Form.Instance },
                                 { "Timeout", (string)request.Form.Timeout }
                             };

            return await _agentConnector.StopApp(address, port, dictionary);
        }

        private async Task<object> RestartApp(IHttpRequest request)
        {
            string address = request.Form.Address;
            int port = request.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "AppName", (string)request.Form.AppName },
                                 { "Version", (string)request.Form.Version },
                                 { "Instance", (string)request.Form.Instance },
                                 { "Timeout", (string)request.Form.Timeout }
                             };

            return await _agentConnector.RestartApp(address, port, dictionary);
        }

        private async Task<object> GetAppsInfo(IHttpRequest request)
        {
            string address = request.Form.Address;
            int port = request.Form.Port;

            var content = await _agentConnector.GetAppsInfo(address, port);

            return new JsonHttpResponse(content,JsonObjectSerializer.Formated);
        }

        private async Task<object> GetConfigurationFile(IHttpRequest request)
        {
            string address = request.Form.Address;
            int port = request.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "AppFullName", (string)request.Form.AppFullName },
                                 { "FileName", (string)request.Form.FileName }
                             };

            return await _agentConnector.GetConfigurationFile(address, port, dictionary);
        }

        private async Task<object> SetConfigurationFile(IHttpRequest request)
        {
            string address = request.Form.Address;
            int port = request.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "Config", (string)request.Form.Config }
                             };

            return await _agentConnector.SetConfigurationFile(address, port, dictionary);
        }

        private async Task<object> GetEnvironmentVariables(IHttpRequest request)
        {
            string address = request.Form.Address;
            int port = request.Form.Port;

            return await _agentConnector.GetVariables(address, port);
        }

        private async Task<object> GetEnvironmentVariable(IHttpRequest request)
        {
            string address = request.Form.Address;
            int port = request.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "Name", (string)request.Form.Name }
                             };

            return await _agentConnector.GetVariable(address, port, dictionary);
        }
    }
}