using System.Collections.Generic;
using System.Threading.Tasks;

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
        }

        private async Task<object> GetAppsInfo(IHttpRequest arg)
        {
            string address = arg.Form.Address;
            int port = arg.Form.Port;


            return await _agentConnector.GetAppsInfo(address, port);
        }

        private async Task<object> InstallApp(IHttpRequest arg)
        {
            string address = arg.Form.Address;
            int port = arg.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "AppName", (string)arg.Form.AppName },
                                 { "Version", (string)arg.Form.Version },
                                 { "Instance", (string)arg.Form.Instance },
                                 { "Source", (string)arg.Form.Source },
                                 { "AllowPrerelease", (string)arg.Form.AllowPrerelease }
                             };

            return await _agentConnector.InstallApp(address, port, dictionary);
        }

        private async Task<object> UninstallApp(IHttpRequest arg)
        {
            string address = arg.Form.Address;
            int port = arg.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "AppName", (string)arg.Form.AppName },
                                 { "Version", (string)arg.Form.Version },
                                 { "Instance", (string)arg.Form.Instance }
                             };

            return await _agentConnector.UninstallApp(address, port, dictionary);
        }

        private async Task<object> InitApp(IHttpRequest arg)
        {
            string address = arg.Form.Address;
            int port = arg.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "AppName", (string)arg.Form.AppName },
                                 { "Version", (string)arg.Form.Version },
                                 { "Instance", (string)arg.Form.Instance },
                                 { "Timeout", (string)arg.Form.Timeout }
                             };

            return await _agentConnector.InitApp(address, port, dictionary);
        }

        private async Task<object> StartApp(IHttpRequest arg)
        {
            string address = arg.Form.Address;
            int port = arg.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "AppName", (string)arg.Form.AppName },
                                 { "Version", (string)arg.Form.Version },
                                 { "Instance", (string)arg.Form.Instance },
                                 { "Timeout", (string)arg.Form.Timeout }
                             };

            return await _agentConnector.StartApp(address, port, dictionary);
        }

        private async Task<object> StopApp(IHttpRequest arg)
        {
            string address = arg.Form.Address;
            int port = arg.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "AppName", (string)arg.Form.AppName },
                                 { "Version", (string)arg.Form.Version },
                                 { "Instance", (string)arg.Form.Instance },
                                 { "Timeout", (string)arg.Form.Timeout }
                             };

            return await _agentConnector.StopApp(address, port, dictionary);
        }

        private async Task<object> RestartApp(IHttpRequest arg)
        {
            string address = arg.Form.Address;
            int port = arg.Form.Port;

            var dictionary = new Dictionary<string, string>
                             {
                                 { "AppName", (string)arg.Form.AppName },
                                 { "Version", (string)arg.Form.Version },
                                 { "Instance", (string)arg.Form.Instance },
                                 { "Timeout", (string)arg.Form.Timeout }
                             };

            return await _agentConnector.RestartApp(address, port, dictionary);
        }

        private async Task<object> GetAgentsStatus(IHttpRequest httpRequest)
        {
            return await _agentConnector.GetAgentsInfo();
        }
    }
}