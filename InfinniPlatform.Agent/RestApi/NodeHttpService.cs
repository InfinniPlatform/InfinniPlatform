using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using InfinniPlatform.Agent.InfinniNode;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Agent.RestApi
{
    /// <summary>
    /// REST-сервис для взаимодействия с утилитой Infinni.Node.
    /// </summary>
    public class NodeHttpService : IHttpService
    {
        public NodeHttpService(INodeConnector nodeConnector)
        {
            _nodeConnector = nodeConnector;
        }

        private readonly INodeConnector _nodeConnector;


        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "node";

            builder.Post["install"] = InstallApp;
            builder.Post["uninstall"] = UninstallApp;
            builder.Post["init"] = InitApp;
            builder.Post["start"] = StartApp;
            builder.Post["stop"] = StopApp;
            builder.Post["restart"] = RestartApp;
            builder.Post["apps"] = GetInstalledAppsInfo;

            builder.Get["apps"] = GetInstalledAppsInfo;
        }

        private async Task<object> InstallApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;
            string version = httpRequest.Form.Version;
            string instance = httpRequest.Form.Instance;
            string source = httpRequest.Form.Source;
            bool? allowPrerelease = httpRequest.Form.AllowPrerelease;

            var processResult = await _nodeConnector.InstallApp(appName, version, instance, source, allowPrerelease);

            return processResult;
        }

        private async Task<object> UninstallApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;
            string version = httpRequest.Form.Version;
            string instance = httpRequest.Form.Instance;

            var processResult = await _nodeConnector.UninstallApp(appName, version, instance);

            return processResult;
        }

        private async Task<object> InitApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;
            string version = httpRequest.Form.Version;
            string instance = httpRequest.Form.Instance;
            int timeout = httpRequest.Form.Instance;

            var processResult = await _nodeConnector.InitApp(appName, version, instance, timeout);

            return processResult;
        }

        private async Task<object> StartApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;

            var processResult = await _nodeConnector.StartApp(appName);

            return processResult;
        }

        private async Task<object> StopApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;

            var processResult = await _nodeConnector.StopApp(appName);

            return processResult;
        }

        private async Task<object> RestartApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;

            var processResult = await _nodeConnector.RestartApp(appName);

            return processResult;
        }

        private async Task<object> GetInstalledAppsInfo(IHttpRequest httpRequest)
        {
            return await _nodeConnector.GetInstalledAppsInfo();
        }
    }
}