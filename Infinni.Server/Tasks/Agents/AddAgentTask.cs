using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;

namespace Infinni.Server.Tasks.Agents
{
    /// <summary>
    /// Добавляет агента.
    /// </summary>
    public class AddAgentTask : IServerTask
    {
        public AddAgentTask(AgentsInfoProvider agentsInfoProvider)
        {
            _agentsInfoProvider = agentsInfoProvider;
        }

        private readonly AgentsInfoProvider _agentsInfoProvider;


        public string CommandName => "addAgent";

        public HttpMethod HttpMethod => HttpMethod.Post;

        public Task<object> Run(IHttpRequest request)
        {
            _agentsInfoProvider.AddInfo(request);

            return Task.FromResult<object>(new ServiceResult<object> { Success = true });
        }
    }
}