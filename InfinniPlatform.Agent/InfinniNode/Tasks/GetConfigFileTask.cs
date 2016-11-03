using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.InfinniNode.Providers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.InfinniNode.Tasks
{
    public class GetConfigFileTask : IAppTask
    {
        public GetConfigFileTask(IConfigurationFileProvider configProvider)
        {
            _configProvider = configProvider;
        }

        private readonly IConfigurationFileProvider _configProvider;

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string CommandName => "config";

        public Task<object> Run(IHttpRequest request)
        {
            {
                string appFullName = request.Query.AppFullName;
                string fileName = request.Query.FileName;

                var configStream = _configProvider.Get(appFullName, fileName);

                return Task.FromResult<object>(new StreamHttpResponse(configStream, "application/json"));
            }
        }
    }
}