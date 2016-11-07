using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.Tasks.InfinniNode
{
    public class StartAppTask : IAppTask
    {
        private const int ProcessTimeout = 10 * 60 * 1000;

        public StartAppTask(InfinniNodeAdapter infinniNodeAdapter)
        {
            _infinniNodeAdapter = infinniNodeAdapter;
        }

        private readonly InfinniNodeAdapter _infinniNodeAdapter;

        public string CommandName => "start";

        public HttpMethod HttpMethod => HttpMethod.Post;

        public async Task<object> Run(IHttpRequest request)
        {
            var command = CommandName.AppendArg("i", (string)request.Form.AppName)
                                     .AppendArg("v", (string)request.Form.Version)
                                     .AppendArg("n", (string)request.Form.Instance)
                                     .AppendArg("t", (string)request.Form.Timeout);

            return await _infinniNodeAdapter.ExecuteCommand(command, ProcessTimeout);
        }
    }
}