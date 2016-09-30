using System.Threading.Tasks;

namespace InfinniPlatform.Agent.InfinniNode
{
    /// <summary>
    /// Интерфейс взаимодействия с утилитой Infinni.Node.
    /// </summary>
    public class Connector : IConnector
    {
        private const int ProcessTimeout = 5 * 60 * 1000;

        public Connector(ProcessHelper processHelper)
        {
            _processHelper = processHelper;
        }

        private readonly ProcessHelper _processHelper;

        public async Task<ProcessHelper.ProcessResult> InstallApp(string appName)
        {
            var args = $"install -i {appName}";

            return await _processHelper.ExecuteCommand(args, ProcessTimeout);
        }

        public async Task<ProcessHelper.ProcessResult> UninstallApp(string appName)
        {
            var args = $"uninstall -i {appName}";

            return await _processHelper.ExecuteCommand(args, ProcessTimeout);
        }

        public async Task<ProcessHelper.ProcessResult> StartApp(string appName)
        {
            var args = $"start -i {appName}";

            return await _processHelper.ExecuteCommand(args, ProcessTimeout);
        }

        public async Task<ProcessHelper.ProcessResult> StopApp(string appName)
        {
            var args = $"stop -i {appName}";

            return await _processHelper.ExecuteCommand(args, ProcessTimeout);
        }
    }
}