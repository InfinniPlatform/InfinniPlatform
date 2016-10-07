using System.Threading.Tasks;

using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Server.Agent
{
    public class ServerHttpService : IHttpService
    {
        public ServerHttpService(IAgentConnector agentConnector)
        {
            _agentConnector = agentConnector;
        }

        private readonly IAgentConnector _agentConnector;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "Server";
            builder.Get["/Agents"] = GetAgentsStatus;
        }

        private Task<object> GetAgentsStatus(IHttpRequest httpRequest)
        {
            var agentsStatus = _agentConnector.GetAgentsStatus();

            return Task.FromResult<object>(agentsStatus);
        }
    }
}