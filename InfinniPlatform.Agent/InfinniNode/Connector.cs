using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public async Task<object> InstallApp(string appName)
        {
            var args = $"install -i {appName}";

            return await _processHelper.ExecuteCommand(args, ProcessTimeout);
        }

        public async Task<object> UninstallApp(string appName)
        {
            var args = $"uninstall -i {appName}";

            return await _processHelper.ExecuteCommand(args, ProcessTimeout);
        }

        public async Task<object> StartApp(string appName)
        {
            var args = $"start -i {appName}";

            return await _processHelper.ExecuteCommand(args, ProcessTimeout);
        }

        public async Task<object> StopApp(string appName)
        {
            var args = $"stop -i {appName}";

            return await _processHelper.ExecuteCommand(args, ProcessTimeout);
        }

        public async Task<object[]> GetInstalledAppsInfo()
        {
            const string args = "status";

            var installedAppsInfo = await _processHelper.ExecuteCommand(args, ProcessTimeout);

            var jsonString = Regex.Replace(installedAppsInfo.Output, OutputRegex, string.Empty, RegexOptions.Multiline, TimeSpan.FromMinutes(1))
                                  .Split(new[] { Environment.NewLine }, StringSplitOptions.None)
                                  .FirstOrDefault(s => s.StartsWith("[{"));

            var appsInfo = _serializer.Deserialize<object[]>(jsonString);

            return appsInfo;
        }
    }
}