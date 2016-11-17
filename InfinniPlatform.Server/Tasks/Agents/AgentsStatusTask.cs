using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Server.Settings;

namespace InfinniPlatform.Server.Tasks.Agents
{
    public class AgentsStatusTask : IServerTask
    {
        public AgentsStatusTask(ServerSettings settings)
        {
            _settings = settings;
        }

        private readonly ServerSettings _settings;

        public string CommandName => "agents";

        public HttpMethod HttpMethod => HttpMethod.Get;

        public Task<object> Run(IHttpRequest request)
        {
            var fileStream = File.OpenRead(_settings.AgentsInfoFilePath);

            using (fileStream)
            {
                var agentsInfo = JsonObjectSerializer.Default.Deserialize<AgentInfo[]>(fileStream);

                var agents = new DynamicWrapper { { "Agents", agentsInfo } };

                var serviceResult = new ServiceResult<DynamicWrapper> { Success = true, Result = agents };

                return Task.FromResult<object>(serviceResult);
            }
        }
    }
}