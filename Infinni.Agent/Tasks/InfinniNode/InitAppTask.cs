using System.Net.Http;
using System.Threading.Tasks;

using Infinni.Agent.Helpers;
using InfinniPlatform.Sdk.Http.Services;

namespace Infinni.Agent.Tasks.InfinniNode
{
    public class InitAppTask : IAgentTask
    {
        private const int ProcessTimeout = 10 * 60 * 1000;

        public InitAppTask(InfinniNodeAdapter infinniNodeAdapter,
                           IAgentTaskStorage agentTaskStorage)
        {
            _infinniNodeAdapter = infinniNodeAdapter;
            _agentTaskStorage = agentTaskStorage;
        }

        private readonly IAgentTaskStorage _agentTaskStorage;
        private readonly InfinniNodeAdapter _infinniNodeAdapter;

        public string CommandName => "init";

        public HttpMethod HttpMethod => HttpMethod.Post;

        public async Task<object> Run(IHttpRequest request)
        {
            var appName = (string)request.Form.AppName;
            var version = (string)request.Form.Version;
            var instanceName = (string)request.Form.Instance;

            var command = CommandName.AppendArg("i", appName)
                                     .AppendArg("v", version)
                                     .AppendArg("n", instanceName)
                                     .AppendArg("t", (string)request.Form.Timeout);

            var description = BuildDescription(appName, version, instanceName);

            var taskId = _agentTaskStorage.AddNewTask(description);

            await _infinniNodeAdapter.ExecuteCommand(command, ProcessTimeout, taskId);

            var serviceResult = new ServiceResult<TaskStatus> { Success = true, Result = _agentTaskStorage.GetTaskStatus(taskId) };

            return Task.FromResult<object>(serviceResult);
        }

        private static string BuildDescription(string appName, string version, string instanceName)
        {
            string result = null;

            if (appName != null)
            {
                result += $"Initializing {appName} ";
            }

            if (version != null)
            {
                result += $"version {version} ";
            }

            if (instanceName != null)
            {
                result += $"with instance name {instanceName}";
            }

            return result?.TrimEnd() + ".";
        }
    }
}