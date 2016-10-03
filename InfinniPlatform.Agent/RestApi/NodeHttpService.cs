using System.Threading.Tasks;

using InfinniPlatform.Agent.InfinniNode;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Agent.RestApi
{
    /// <summary>
    /// REST-сервис для
    /// </summary>
    public class NodeHttpService : IHttpService
    {
        public NodeHttpService(IConnector connector)
        {
            _connector = connector;
        }

        private readonly IConnector _connector;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "node";
            builder.Post["install"] = InstallApp;
            builder.Post["uninstall"] = UninstallApp;
            builder.Post["start"] = StartApp;
            builder.Post["stop"] = StopApp;
            builder.Get["apps"] = GetInstalledApps;
        }

        private async Task<object> InstallApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;

            var processResult = await _connector.InstallApp(appName);

            return processResult;
        }

        private async Task<object> UninstallApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;

            var processResult = await _connector.UninstallApp(appName);

            return processResult;
        }

        private async Task<object> StartApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;

            var processResult = await _connector.StartApp(appName);

            return processResult;
        }

        private async Task<object> StopApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;

            var processResult = await _connector.StopApp(appName);

            return processResult;
        }

        private async Task<object> GetInstalledApps(IHttpRequest httpRequest)
        {
            var processResult = await _connector.GetInstalledApps();

            return processResult;
        }
    }
}