using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.Tasks.InfinniNode
{
    public class InstallAppTask : IAppTask
    {
        private const int ProcessTimeout = 10 * 60 * 1000;

        public InstallAppTask(InfinniNodeAdapter infinniNodeAdapter,
                              INodeTaskStorage nodeTaskStorage)
        {
            _infinniNodeAdapter = infinniNodeAdapter;
            _nodeTaskStorage = nodeTaskStorage;
        }

        private readonly InfinniNodeAdapter _infinniNodeAdapter;
        private readonly INodeTaskStorage _nodeTaskStorage;

        public string CommandName => "install";

        public HttpMethod HttpMethod => HttpMethod.Post;

        public Task<object> Run(IHttpRequest request)
        {
            var command = CommandName.AppendArg("i", (string)request.Form.AppName)
                                     .AppendArg("v", (string)request.Form.Version)
                                     .AppendArg("n", (string)request.Form.instance)
                                     .AppendArg("s", (string)request.Form.Source)
                                     .AppendArg("p", (bool?)request.Form.AllowPrerelease);


            var taskId = _nodeTaskStorage.AddNewTask();

            Task.Run(async () => { await _infinniNodeAdapter.ExecuteCommand(command, ProcessTimeout, taskId); });

            return Task.FromResult<object>(taskId);
        }
    }
}