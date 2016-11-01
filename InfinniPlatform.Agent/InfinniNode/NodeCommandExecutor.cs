using System;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Helpers;

namespace InfinniPlatform.Agent.InfinniNode
{
    /// <summary>
    /// Интерфейс взаимодействия с приложением Infinni.Node.
    /// </summary>
    public class NodeCommandExecutor : INodeCommandExecutor
    {
        private const int ProcessTimeout = 5 * 60 * 1000;
        private const string InstallCommand = "install";
        private const string UninstallCommand = "uninstall";
        private const string InitCommand = "init";
        private const string StartCommand = "start";
        private const string StopCommand = "stop";
        private const string RestartCommand = "restart";
        private const string StatusCommand = "status";

        public NodeCommandExecutor(ProcessHelper processHelper,
                                   ITaskOutputHandler taskOutputHandler)
        {
            _processHelper = processHelper;
            _processHelper.OnNodeOutputDataRecieved += async (s, args) => { await taskOutputHandler.Handle(args); };
        }

        private readonly ProcessHelper _processHelper;

        public async Task<ProcessHelper.ProcessResult> InstallApp(string appName, string version = null, string instance = null, string source = null, bool? allowPrerelease = null)
        {
            var command = InstallCommand.AppendArg("i", appName)
                                        .AppendArg("v", version)
                                        .AppendArg("n", instance)
                                        .AppendArg("s", source)
                                        .AppendArg("p", allowPrerelease);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout, Guid.NewGuid().ToString("D"));

            return processResult;
        }

        public async Task<ProcessHelper.ProcessResult> UninstallApp(string appName, string version = null, string instance = null)
        {
            var command = UninstallCommand.AppendArg("i", appName)
                                          .AppendArg("v", version)
                                          .AppendArg("n", instance);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout, Guid.NewGuid().ToString("D"));

            return processResult;
        }

        public async Task<ProcessHelper.ProcessResult> InitApp(string appName, string version = null, string instance = null, int? timeout = null)
        {
            var command = InitCommand.AppendArg("i", appName)
                                     .AppendArg("v", version)
                                     .AppendArg("n", instance)
                                     .AppendArg("t", timeout);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout, Guid.NewGuid().ToString("D"));

            return processResult;
        }

        public async Task<ProcessHelper.ProcessResult> StartApp(string appName, string version = null, string instance = null, int? timeout = null)
        {
            var command = StartCommand.AppendArg("i", appName)
                                      .AppendArg("v", version)
                                      .AppendArg("n", instance)
                                      .AppendArg("t", timeout);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout, Guid.NewGuid().ToString("D"));

            return processResult;
        }

        public async Task<ProcessHelper.ProcessResult> StopApp(string appName, string version = null, string instance = null, int? timeout = null)
        {
            var command = StopCommand.AppendArg("i", appName)
                                     .AppendArg("v", version)
                                     .AppendArg("n", instance)
                                     .AppendArg("t", timeout);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout, Guid.NewGuid().ToString("D"));

            return processResult;
        }

        public async Task<ProcessHelper.ProcessResult> RestartApp(string appName, string version = null, string instance = null, int? timeout = null)
        {
            var command = RestartCommand.AppendArg("i", appName)
                                        .AppendArg("v", version)
                                        .AppendArg("n", instance)
                                        .AppendArg("t", timeout);

            var processResult = await _processHelper.ExecuteCommand(command, ProcessTimeout, Guid.NewGuid().ToString("D"));

            return processResult;
        }

        public async Task<ProcessHelper.ProcessResult> GetInstalledAppsInfo()
        {
            var processResult = await _processHelper.ExecuteCommand(StatusCommand, ProcessTimeout, Guid.NewGuid().ToString("D"));

            return processResult;
        }
    }
}