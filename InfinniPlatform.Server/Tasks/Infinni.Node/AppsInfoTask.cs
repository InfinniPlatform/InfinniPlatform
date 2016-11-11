using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Server.Agent;
using InfinniPlatform.Server.Tasks.Agents;

namespace InfinniPlatform.Server.Tasks.Infinni.Node
{
    public class AppsInfoTask : IServerTask
    {
        public AppsInfoTask(IAgentHttpClient agentHttpClient,
                            INodeOutputParser nodeOutputParser)
        {
            _agentHttpClient = agentHttpClient;
            _nodeOutputParser = nodeOutputParser;
        }

        private readonly IAgentHttpClient _agentHttpClient;
        private readonly INodeOutputParser _nodeOutputParser;

        public string CommandName => "appsInfo";

        public HttpMethod HttpMethod => HttpMethod.Get;

        public async Task<object> Run(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var serviceResult = await _agentHttpClient.Get<ServiceResult<AgentTaskStatus>>(CommandName, address, port);

            var appsInfo = _nodeOutputParser.FormatAppsInfoOutput(serviceResult);

            return appsInfo;
        }
    }
}