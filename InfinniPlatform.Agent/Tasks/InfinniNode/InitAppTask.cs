using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.Tasks.InfinniNode
{
    public class InitAppTask : IAgentTask
    {
        private const int ProcessTimeout = 10 * 60 * 1000;

        public InitAppTask(InfinniNodeAdapter infinniNodeAdapter,
                           ITaskStorage taskStorage)
        {
            _infinniNodeAdapter = infinniNodeAdapter;
            _taskStorage = taskStorage;
        }

        private readonly InfinniNodeAdapter _infinniNodeAdapter;
        private readonly ITaskStorage _taskStorage;

        public string CommandName => "init";

        public HttpMethod HttpMethod => HttpMethod.Post;

        public Task<object> Run(IHttpRequest request)
        {
            var appName = (string)request.Form.AppName;
            var version = (string)request.Form.Version;
            var instanceName = (string)request.Form.Instance;

            var command = CommandName.AppendArg("i", appName)
                                     .AppendArg("v", version)
                                     .AppendArg("n", instanceName)
                                     .AppendArg("t", (string)request.Form.Timeout);

            var description = $"Initializing {appName} version {version} with instance name {instanceName}.";

            var taskId = _taskStorage.AddNewTask(description);

            Task.Run(async () => { await _infinniNodeAdapter.ExecuteCommand(command, ProcessTimeout, taskId); });

            var result = new DynamicWrapper { { "TaskId", taskId } };

            var serviceResult = new ServiceResult<DynamicWrapper> { Success = true, Result = result };

            return Task.FromResult<object>(serviceResult);
        }
    }
}