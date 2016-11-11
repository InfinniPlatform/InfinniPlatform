using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Server.Agent;
using InfinniPlatform.Server.Tasks.Agents;

namespace InfinniPlatform.Server.Tasks.Infinni.Node
{
    public class PackagesTask : IServerTask
    {
        public PackagesTask(IAgentHttpClient agentHttpClient,
                            INodeOutputParser nodeOutputParser)
        {
            _agentHttpClient = agentHttpClient;
            _nodeOutputParser = nodeOutputParser;
        }

        private readonly IAgentHttpClient _agentHttpClient;
        private readonly INodeOutputParser _nodeOutputParser;

        public string CommandName => "packages";

        public HttpMethod HttpMethod => HttpMethod.Get;

        public async Task<object> Run(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var args = new DynamicWrapper
                       {
                           { "SearchTerm", HttpServiceHelper.ParseString(request.Query.SearchTerm) },
                           { "Prerelease", HttpServiceHelper.ParseBool(request.Query.Prerelease)  }
                       };

            var serviceResult = await _agentHttpClient.Get<ServiceResult<AgentTaskStatus>>(CommandName, address, port, args);

            var formattedServiceResult = _nodeOutputParser.FormatPackagesOutput(serviceResult);

            return formattedServiceResult;
        }
    }
}