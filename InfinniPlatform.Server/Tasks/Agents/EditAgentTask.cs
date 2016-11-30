using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Server.Tasks.Agents
{
    /// <summary>
    /// Изменяет информацию об агенте.
    /// </summary>
    public class EditAgentTask : IServerTask
    {
        public EditAgentTask(AgentsInfoProvider agentsInfoProvider)
        {
            _agentsInfoProvider = agentsInfoProvider;
        }

        private readonly AgentsInfoProvider _agentsInfoProvider;


        public string CommandName => "editAgent";

        public HttpMethod HttpMethod => HttpMethod.Post;

        public Task<object> Run(IHttpRequest request)
        {
            _agentsInfoProvider.EditInfo(request);

            return Task.FromResult<object>(new ServiceResult<object> { Success = true });
        }
    }
}