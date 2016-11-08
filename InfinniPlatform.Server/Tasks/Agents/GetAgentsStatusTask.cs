using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Server.Settings;

namespace InfinniPlatform.Server.Tasks.Agents
{
    public class GetAgentsStatusTask : IServerTask
    {
        public GetAgentsStatusTask(ServerSettings settings)
        {
            _settings = settings;
        }

        private readonly ServerSettings _settings;

        public string CommandName => "agents";

        public HttpMethod HttpMethod => HttpMethod.Get;

        public Task<object> Run(IHttpRequest request)
        {
            var agents = new DynamicWrapper { { "Agents", _settings.AgentsInfo } };

            var serviceResult = new ServiceResult<DynamicWrapper> { Success = true, Result = agents };

            return Task.FromResult<object>(serviceResult);
        }
    }
}