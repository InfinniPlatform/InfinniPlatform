using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Providers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.Tasks.Files
{
    public class AppLogTask : IAgentTask
    {
        public AppLogTask(ILogFilePovider logFilePovider)
        {
            _logFilePovider = logFilePovider;
        }

        private readonly ILogFilePovider _logFilePovider;

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string CommandName => "appLog";

        public Task<object> Run(IHttpRequest request)
        {
            string appFullName = request.Query.FullName;

            var streamHttpResponse = new StreamHttpResponse(_logFilePovider.GetAppLog(appFullName));

            return Task.FromResult<object>(streamHttpResponse);
        }
    }
}