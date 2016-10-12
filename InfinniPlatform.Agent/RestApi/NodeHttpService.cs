using System.Threading.Tasks;

using InfinniPlatform.Agent.InfinniNode;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Agent.RestApi
{
    /// <summary>
    /// Сервис взаимодействия с утилитой Infinni.Node.
    /// </summary>
    public class NodeHttpService : IHttpService
    {
        public NodeHttpService(INodeConnector nodeConnector,
                               IConfigurationFileProvider configProvider,
                               IEnvironmentVariableProvider variableProvider)
        {
            _nodeConnector = nodeConnector;
            _configProvider = configProvider;
            _variableProvider = variableProvider;
        }

        private readonly IConfigurationFileProvider _configProvider;
        private readonly INodeConnector _nodeConnector;
        private readonly IEnvironmentVariableProvider _variableProvider;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "node";

            builder.Post["install"] = InstallApp;
            builder.Post["uninstall"] = UninstallApp;

            builder.Post["init"] = InitApp;
            builder.Post["start"] = StartApp;
            builder.Post["stop"] = StopApp;
            builder.Post["restart"] = RestartApp;

            builder.Get["appsInfo"] = GetInstalledAppsInfo;

            builder.Get["config"] = GetConfigurationFile;
            builder.Post["config"] = SetConfigurationFile;

            builder.Get["variables"] = GetEnvironmentVariables;
            builder.Get["variable"] = GetEnvironmentVariable;
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
            int? timeout = httpRequest.Form.Timeout;

            var processResult = await _nodeConnector.InitApp(appName, version, instance, timeout);

            return processResult;
        }

        private async Task<object> StartApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;
            string version = httpRequest.Form.Version;
            string instance = httpRequest.Form.Instance;
            int? timeout = httpRequest.Form.Timeout;

            var processResult = await _nodeConnector.StartApp(appName, version, instance, timeout);

            return processResult;
        }

        private async Task<object> StopApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;
            string version = httpRequest.Form.Version;
            string instance = httpRequest.Form.Instance;
            int? timeout = httpRequest.Form.Timeout;

            var processResult = await _nodeConnector.StopApp(appName, version, instance, timeout);

            return processResult;
        }

        private async Task<object> RestartApp(IHttpRequest httpRequest)
        {
            string appName = httpRequest.Form.AppName;
            string version = httpRequest.Form.Version;
            string instance = httpRequest.Form.Instance;
            int? timeout = httpRequest.Form.Timeout;

            var processResult = await _nodeConnector.RestartApp(appName, version, instance, timeout);

            return processResult;
        }

        private async Task<object> GetInstalledAppsInfo(IHttpRequest httpRequest)
        {
            return await _nodeConnector.GetInstalledAppsInfo();
        }

        private Task<object> GetConfigurationFile(IHttpRequest httpRequest)
        {
            string appFullName = httpRequest.Query.AppFullName;
            string fileName = httpRequest.Query.FileName;

            var configStreamResponse = _configProvider.Get(appFullName, fileName);

            return Task.FromResult<object>(configStreamResponse);
        }

        private Task<object> SetConfigurationFile(IHttpRequest httpRequest)
        {
            string appFullName = httpRequest.Query.AppFullName;
            string fileName = httpRequest.Query.FileName;
            string content = httpRequest.Form.Content;

            _configProvider.Set(appFullName, fileName, content);

            return Task.FromResult<object>("Ok.");
        }

        private Task<object> GetEnvironmentVariables(IHttpRequest httpRequest)
        {
            var variables = _variableProvider.GetAll();

            return Task.FromResult<object>(new JsonHttpResponse(variables, JsonObjectSerializer.Formated));
        }

        private Task<object> GetEnvironmentVariable(IHttpRequest httpRequest)
        {
            string name = httpRequest.Query.Name;

            var variable = _variableProvider.Get(name);

            return Task.FromResult<object>(new JsonHttpResponse(new DynamicWrapper
                                                                {
                                                                    { name, variable }
                                                                }, JsonObjectSerializer.Formated));
        }
    }
}