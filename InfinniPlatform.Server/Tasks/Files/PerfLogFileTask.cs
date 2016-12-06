using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Server.Agent;

namespace InfinniPlatform.Server.Tasks.Files
{
    public class PerfLogFileTask : IServerTask
    {
        public PerfLogFileTask(IAgentHttpClient agentHttpClient)
        {
            _agentHttpClient = agentHttpClient;
        }

        private readonly IAgentHttpClient _agentHttpClient;

        public string CommandName => "performance.log";

        public HttpMethod HttpMethod => HttpMethod.Get;

        public async Task<object> Run(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "FullName", (string)request.Query.FullName }
                            };

            var stream = await _agentHttpClient.GetStream("perfLog", address, port, arguments);

            var streamHttpResponse = new StreamHttpResponse(() => stream, HttpConstants.TextContentType);

            return streamHttpResponse;
        }
    }
}