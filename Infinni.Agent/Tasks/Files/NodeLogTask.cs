using System.Net.Http;
using System.Threading.Tasks;

using Infinni.Agent.Providers;
using InfinniPlatform.Sdk.Http.Services;

namespace Infinni.Agent.Tasks.Files
{
    public class NodeLogTask : IAgentTask
    {
        public NodeLogTask(ILogFilePovider logFilePovider)
        {
            _logFilePovider = logFilePovider;
        }

        private readonly ILogFilePovider _logFilePovider;

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string CommandName => "nodeLog";

        public Task<object> Run(IHttpRequest request)
        {
            var streamHttpResponse = new StreamHttpResponse(_logFilePovider.GetNodeLog(), "application/text");
            return Task.FromResult<object>(streamHttpResponse);
        }
    }
}