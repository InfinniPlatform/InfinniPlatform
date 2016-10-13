using System.Threading.Tasks;

using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Agent.InfinniNode
{
    /// <summary>
    /// Интерфейс взаимодействия с утилитой Infinni.Node.
    /// </summary>
    public class NodeConnector : INodeConnector
    {
        private const int ProcessTimeout = 5 * 60 * 1000;

        public NodeConnector(ProcessHelper processHelper,
                             IJsonObjectSerializer serializer)
        {
            _processHelper = processHelper;
            _serializer = serializer;
        }

        private readonly ProcessHelper _processHelper;
        private readonly IJsonObjectSerializer _serializer;

        public async Task<ProcessHelper.ProcessResult> InstallApp(string appName, string version = null, string instance = null, string source = null, bool? allowPrerelease = null)
        {
            var command = "install";

            command = command.AppendArg("i", appName)
                             .AppendArg("v", version)
                             .AppendArg("n", instance)
                             .AppendArg("s", source)
                             .AppendArg("p", allowPrerelease);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout);

            return processResult;
        }

        public async Task<ProcessHelper.ProcessResult> UninstallApp(string appName, string version = null, string instance = null)
        {
            var command = "uninstall";

            command = command.AppendArg("i", appName)
                             .AppendArg("v", version)
                             .AppendArg("n", instance);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout);

            return processResult;
        }

        public async Task<ProcessHelper.ProcessResult> InitApp(string appName, string version = null, string instance = null, int? timeout = null)
        {
            var command = "init";

            command = command.AppendArg("i", appName)
                             .AppendArg("v", version)
                             .AppendArg("n", instance)
                             .AppendArg("t", timeout);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout);

            return processResult;
        }

        public async Task<ProcessHelper.ProcessResult> StartApp(string appName, string version = null, string instance = null, int? timeout = null)
        {
            var command = "start";

            command = command.AppendArg("i", appName)
                             .AppendArg("v", version)
                             .AppendArg("n", instance)
                             .AppendArg("t", timeout);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout);

            return processResult;
        }

        public async Task<ProcessHelper.ProcessResult> StopApp(string appName, string version = null, string instance = null, int? timeout = null)
        {
            var command = "stop";

            command = command.AppendArg("i", appName)
                             .AppendArg("v", version)
                             .AppendArg("n", instance)
                             .AppendArg("t", timeout);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout);

            return processResult;
        }

        public async Task<ProcessHelper.ProcessResult> RestartApp(string appName, string version = null, string instance = null, int? timeout = null)
        {
            var command = "restart";

            command = command.AppendArg("i", appName)
                             .AppendArg("v", version)
                             .AppendArg("n", instance)
                             .AppendArg("t", timeout);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout);

            return processResult;
        }

        public async Task<ProcessHelper.ProcessResult> GetInstalledAppsInfo()
        {
            const string command = "status";

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout);

            return processResult;
        }
    }
}