using System.Net.Http;
using System.Threading.Tasks;

using Infinni.Agent.Providers;
using InfinniPlatform.Sdk.Http.Services;

namespace Infinni.Agent.Tasks.Files
{
    public class ConfigGetTask : IAgentTask
    {
        public ConfigGetTask(IConfigurationFileProvider configProvider)
        {
            _configProvider = configProvider;
        }

        private readonly IConfigurationFileProvider _configProvider;

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string CommandName => "config";

        public Task<object> Run(IHttpRequest request)
        {
            string appFullName = request.Query.FullName;
            string fileName = request.Query.FileName;

            var configStream = _configProvider.Get(appFullName, fileName);

            return Task.FromResult<object>(new StreamHttpResponse(configStream, "application/json"));
        }
    }
}