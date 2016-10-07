using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Agent.InfinniNode
{
    /// <summary>
    /// Интерфейс взаимодействия с утилитой Infinni.Node.
    /// </summary>
    public class Connector : IConnector
    {
        private const int ProcessTimeout = 5 * 60 * 1000;
        private const string OutputRegex = "\\d{4}-\\d{2}-\\d{2}\\s\\d{2}:\\d{2}:\\d{2},\\d{3}\\s\\[PID\\s\\d{1,10}\\]\\sINFO\\s{1,4}-\\s";

        public Connector(ProcessHelper processHelper,
                         IJsonObjectSerializer serializer)
        {
            _processHelper = processHelper;
            _serializer = serializer;
        }

        private readonly ProcessHelper _processHelper;
        private readonly IJsonObjectSerializer _serializer;

        public async Task<object> InstallApp(string appName, string version = null, string instance = null, string source = null, bool? allowPrerelease = null)
        {
            var command = "install";

            command = command.AppendArg("i", appName)
                             .AppendArg("v", version)
                             .AppendArg("n", instance)
                             .AppendArg("s", source)
                             .AppendArg("p", allowPrerelease);

            return await _processHelper.ExecuteCommand(command, ProcessTimeout);
        }

        public async Task<object> UninstallApp(string appName, string version = null, string instance = null)
        {
            var command = "uninstall";

            command = command.AppendArg("i", appName)
                             .AppendArg("v", version)
                             .AppendArg("n", instance);

            return await _processHelper.ExecuteCommand(command, ProcessTimeout);
        }

        public async Task<object> StartApp(string appName, string version = null, string instance = null, string timeout = null)
        {
            var command = "start";

            command = command.AppendArg("i", appName)
                             .AppendArg("v", version)
                             .AppendArg("n", instance)
                             .AppendArg("t", timeout);

            return await _processHelper.ExecuteCommand(command, ProcessTimeout);
        }

        public async Task<object> StopApp(string appName, string version = null, string instance = null, string timeout = null)
        {
            var command = "stop";

            command = command.AppendArg("i", appName)
                             .AppendArg("v", version)
                             .AppendArg("n", instance)
                             .AppendArg("t", timeout);

            return await _processHelper.ExecuteCommand(command, ProcessTimeout);
        }

        public async Task<object[]> GetInstalledAppsInfo()
        {
            const string command = "status";

            var installedAppsInfo = await _processHelper.ExecuteCommand(command, ProcessTimeout);

            var jsonString = Regex.Replace(installedAppsInfo.Output, OutputRegex, string.Empty, RegexOptions.Multiline, TimeSpan.FromMinutes(1))
                                  .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                                  .FirstOrDefault(s => s.StartsWith("[{"));

            var appsInfo = _serializer.Deserialize<object[]>(jsonString);

            return appsInfo;
        }


        public async Task<object> InitApp(string appName, string version = null, string instance = null, string timeout = null)
        {
            var command = "init";

            command = command.AppendArg("i", appName)
                             .AppendArg("v", version)
                             .AppendArg("n", instance)
                             .AppendArg("t", timeout);

            return await _processHelper.ExecuteCommand(command, ProcessTimeout);
        }

        public async Task<object> RestartApp(string appName, string version = null, string instance = null, string timeout = null)
        {
            var command = "restart";

            command = command.AppendArg("i", appName)
                             .AppendArg("v", version)
                             .AppendArg("n", instance)
                             .AppendArg("t", timeout);

            return await _processHelper.ExecuteCommand(command, ProcessTimeout);
        }
    }
}