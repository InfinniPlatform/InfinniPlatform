using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Serialization;
using Infinni.Server.Agent;

namespace Infinni.Server.Tasks.Files
{
    public class ConfigGetTask : IServerTask
    {
        public ConfigGetTask(IAgentHttpClient agentHttpClient)
        {
            _agentHttpClient = agentHttpClient;
        }

        private readonly IAgentHttpClient _agentHttpClient;

        public string CommandName => "config";

        public HttpMethod HttpMethod => HttpMethod.Get;

        public async Task<object> Run(IHttpRequest request)
        {
            var serviceResult = new ServiceResult<string>();

            string address = request.Query.Address;
            int port = request.Query.Port;

            var arguments = new DynamicWrapper
                            {
                                { "FullName", (string)request.Query.FullName },
                                { "FileName", (string)request.Query.FileName }
                            };

            using (var stream = await _agentHttpClient.GetStream("config", address, port, arguments))
            {
                serviceResult.Result = CleanUpJsonConfig(stream);
            }

            return serviceResult;
        }

        private static string CleanUpJsonConfig(Stream stream)
        {
            var configObject = JsonObjectSerializer.Formated.Deserialize(stream);
            var cleanConfigString = JsonObjectSerializer.Formated.ConvertToString(configObject);

            return cleanConfigString;
        }
    }
}