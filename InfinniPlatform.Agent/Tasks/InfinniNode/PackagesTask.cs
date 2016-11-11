using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.Tasks.InfinniNode
{
    public class PackagesTask : IAgentTask
    {
        private const int ProcessTimeout = 10 * 60 * 1000;

        public PackagesTask(InfinniNodeAdapter infinniNodeAdapter)
        {
            _infinniNodeAdapter = infinniNodeAdapter;
        }

        private readonly InfinniNodeAdapter _infinniNodeAdapter;

        public string CommandName => "packages";

        public HttpMethod HttpMethod => HttpMethod.Get;

        public async Task<object> Run(IHttpRequest request)
        {
            var searchTerm = (string)request.Query.SearchTerm;
            var prerelease = (bool?)request.Query.Prerelease;

            var command = CommandName.AppendArg("i", searchTerm)
                                     .AppendArg("p", prerelease);

            var result = await _infinniNodeAdapter.ExecuteCommand(command, ProcessTimeout);

            return new ServiceResult<TaskStatus> { Success = true, Result = result };
        }
    }
}