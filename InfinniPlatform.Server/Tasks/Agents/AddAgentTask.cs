using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Server.Settings;
using InfinniPlatform.Server.Tasks.Infinni.Node;

namespace InfinniPlatform.Server.Tasks.Agents
{
    public class AddAgentTask : IServerTask
    {
        public AddAgentTask(ServerSettings settings,
                            IJsonObjectSerializer serializer)
        {
            _settings = settings;
            _serializer = serializer;
        }

        private readonly IJsonObjectSerializer _serializer;
        private readonly ServerSettings _settings;

        public string CommandName => "addAgent";

        public HttpMethod HttpMethod => HttpMethod.Post;

        public Task<object> Run(IHttpRequest request)
        {
            var agentsInfoString = File.ReadAllText(_settings.AgentsInfoFilePath);

            var agentsInfo = _serializer.Deserialize<List<AgentInfo>>(agentsInfoString);
            agentsInfo.Add(CreateAgentInfo(request));

            File.WriteAllBytes(_settings.AgentsInfoFilePath, _serializer.Serialize(agentsInfo));

            return Task.FromResult<object>(new ServiceResult<object> { Success = true });
        }

        private static AgentInfo CreateAgentInfo(IHttpRequest request)
        {
            string address = request.Form.Address;
            int port = HttpServiceHelper.ParseInt(request.Form.Port);
            string name = request.Form.Name;

            return new AgentInfo(name, address, port);
        }
    }
}