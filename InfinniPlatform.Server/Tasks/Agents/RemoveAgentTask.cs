using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Server.Tasks.Agents
{
    /// <summary>
    /// Удаляет информацию об агенте.
    /// </summary>
    public class RemoveAgentTask : IServerTask
    {
        public RemoveAgentTask(AgentsInfoProvider agentsInfoProvider)
        {
            _agentsInfoProvider = agentsInfoProvider;
        }

        private readonly AgentsInfoProvider _agentsInfoProvider;


        public string CommandName => "removeAgent";

        public HttpMethod HttpMethod => HttpMethod.Post;

        public Task<object> Run(IHttpRequest request)
        {
            _agentsInfoProvider.RemoveInfo(request);

            return Task.FromResult<object>(new ServiceResult<object> { Success = true });
        }
    }
}