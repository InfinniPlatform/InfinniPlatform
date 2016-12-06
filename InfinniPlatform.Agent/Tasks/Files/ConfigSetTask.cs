using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Agent.Providers;
using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.Tasks.Files
{
    public class ConfigSetTask : IAgentTask
    {
        public ConfigSetTask(IConfigurationFileProvider configProvider)
        {
            _configProvider = configProvider;
        }

        private readonly IConfigurationFileProvider _configProvider;

        public HttpMethod HttpMethod => HttpMethod.Post;

        public string CommandName => "config";

        public Task<object> Run(IHttpRequest request)
        {
            string appFullName = request.Form.FullName;
            string fileName = request.Form.FileName;
            string config = request.Form.Config;

            _configProvider.Set(appFullName, fileName, config);

            return Task.FromResult<object>(new ServiceResult<object> { Success = true });
        }
    }
}