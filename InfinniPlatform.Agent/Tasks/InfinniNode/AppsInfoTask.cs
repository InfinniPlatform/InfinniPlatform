using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Helpers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.Tasks.InfinniNode
{
    public class AppsInfoTask : IAppTask
    {
        private const int ProcessTimeout = 10 * 60 * 1000;

        public AppsInfoTask(InfinniNodeAdapter infinniNodeAdapter)
        {
            _infinniNodeAdapter = infinniNodeAdapter;
        }

        private readonly InfinniNodeAdapter _infinniNodeAdapter;

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string CommandName => "appsInfo";

        public async Task<object> Run(IHttpRequest request)
        {
            return await _infinniNodeAdapter.ExecuteCommand("status", ProcessTimeout);
        }
    }
}