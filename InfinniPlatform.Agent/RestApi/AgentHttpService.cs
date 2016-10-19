using System.Threading.Tasks;

using InfinniPlatform.Agent.InfinniNode;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Agent.RestApi
{
    /// <summary>
    /// Сервис взаимодействия с утилитой Infinni.Node.
    /// </summary>
    public class AgentHttpService : IHttpService
    {
        public AgentHttpService(INodeCommandExecutor nodeCommandExecutor,
                                IConfigurationFileProvider configProvider,
                                IEnvironmentVariableProvider variableProvider,
                                ILogFilePovider logFilePovider)
        {
            _nodeCommandExecutor = nodeCommandExecutor;
            _configProvider = configProvider;
            _variableProvider = variableProvider;
            _logFilePovider = logFilePovider;
        }

        private readonly IConfigurationFileProvider _configProvider;
        private readonly ILogFilePovider _logFilePovider;
        private readonly INodeCommandExecutor _nodeCommandExecutor;
        private readonly IEnvironmentVariableProvider _variableProvider;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "agent";

            builder.Post["install"] = InstallApp;
            builder.Post["uninstall"] = UninstallApp;

            builder.Post["init"] = InitApp;
            builder.Post["start"] = StartApp;
            builder.Post["stop"] = StopApp;
            builder.Post["restart"] = RestartApp;

            builder.Get["appsInfo"] = GetInstalledAppsInfo;

            builder.Get["config"] = GetConfigurationFile;
            builder.Post["config"] = SetConfigurationFile;

            builder.Get["appLog"] = GetAppLogFile;
            builder.Get["perfLog"] = GetPerfLogFile;
            builder.Get["nodeLog"] = GetNodeLogFile;

            builder.Get["variables"] = GetEnvironmentVariables;
            builder.Get["variable"] = GetEnvironmentVariable;
        }

        private async Task<object> InstallApp(IHttpRequest request)
        {
            string appName = request.Form.AppName;
            string version = request.Form.Version;
            string instance = request.Form.Instance;
            string source = request.Form.Source;
            bool? allowPrerelease = request.Form.AllowPrerelease;

            var processResult = await _nodeCommandExecutor.InstallApp(appName, version, instance, source, allowPrerelease);

            return processResult;
        }

        private async Task<object> UninstallApp(IHttpRequest request)
        {
            string appName = request.Form.AppName;
            string version = request.Form.Version;
            string instance = request.Form.Instance;

            var processResult = await _nodeCommandExecutor.UninstallApp(appName, version, instance);

            return processResult;
        }

        private async Task<object> InitApp(IHttpRequest request)
        {
            string appName = request.Form.AppName;
            string version = request.Form.Version;
            string instance = request.Form.Instance;
            int? timeout = request.Form.Timeout;

            var processResult = await _nodeCommandExecutor.InitApp(appName, version, instance, timeout);

            return processResult;
        }

        private async Task<object> StartApp(IHttpRequest request)
        {
            string appName = request.Form.AppName;
            string version = request.Form.Version;
            string instance = request.Form.Instance;
            int? timeout = request.Form.Timeout;

            var processResult = await _nodeCommandExecutor.StartApp(appName, version, instance, timeout);

            return processResult;
        }

        private async Task<object> StopApp(IHttpRequest request)
        {
            string appName = request.Form.AppName;
            string version = request.Form.Version;
            string instance = request.Form.Instance;
            int? timeout = request.Form.Timeout;

            var processResult = await _nodeCommandExecutor.StopApp(appName, version, instance, timeout);

            return processResult;
        }

        private async Task<object> RestartApp(IHttpRequest request)
        {
            string appName = request.Form.AppName;
            string version = request.Form.Version;
            string instance = request.Form.Instance;
            int? timeout = request.Form.Timeout;

            var processResult = await _nodeCommandExecutor.RestartApp(appName, version, instance, timeout);

            return processResult;
        }

        private async Task<object> GetInstalledAppsInfo(IHttpRequest request)
        {
            return await _nodeCommandExecutor.GetInstalledAppsInfo();
        }

        private Task<object> GetConfigurationFile(IHttpRequest request)
        {
            string appFullName = request.Query.AppFullName;
            string fileName = request.Query.FileName;

            var configStreamResponse = _configProvider.Get(appFullName, fileName);

            return Task.FromResult<object>(configStreamResponse);
        }

        private Task<object> SetConfigurationFile(IHttpRequest request)
        {
            string appFullName = request.Query.AppFullName;
            string fileName = request.Query.FileName;
            string content = request.Form.Content;

            _configProvider.Set(appFullName, fileName, content);

            return Task.FromResult<object>("Ok.");
        }

        private Task<object> GetEnvironmentVariables(IHttpRequest request)
        {
            var variables = _variableProvider.GetAll();

            return Task.FromResult<object>(variables);
        }

        private Task<object> GetEnvironmentVariable(IHttpRequest request)
        {
            string name = request.Query.Name;

            var variable = _variableProvider.Get(name);

            return Task.FromResult<object>(variable);
        }

        private Task<object> GetAppLogFile(IHttpRequest request)
        {
            string fullAppName = request.Query.FullAppName;

            var streamHttpResponse = new StreamHttpResponse(() => _logFilePovider.GetAppLog(fullAppName), "application/text");

            return Task.FromResult<object>(streamHttpResponse);
        }

        private Task<object> GetPerfLogFile(IHttpRequest request)
        {
            string fullAppName = request.Query.FullAppName;

            var streamHttpResponse = new StreamHttpResponse(() => _logFilePovider.GetPerformanceLog(fullAppName), "application/text");

            return Task.FromResult<object>(streamHttpResponse);
        }

        private Task<object> GetNodeLogFile(IHttpRequest request)
        {
            var streamHttpResponse = new StreamHttpResponse(() => _logFilePovider.GetNodeLog(), "application/text");

            return Task.FromResult<object>(streamHttpResponse);
        }
    }
}