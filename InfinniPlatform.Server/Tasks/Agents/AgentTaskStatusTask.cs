using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Server.Agent;

namespace InfinniPlatform.Server.Tasks.Agents
{
    public class AgentTaskStatusTask : IServerTask
    {
        public AgentTaskStatusTask(IAgentHttpClient agentHttpClient)
        {
            _agentHttpClient = agentHttpClient;
        }

        private readonly IAgentHttpClient _agentHttpClient;

        public string CommandName => "taskStatus";

        public HttpMethod HttpMethod => HttpMethod.Get;

        public async Task<object> Run(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var taskId = (string)request.Query.TaskId;

            if (taskId == null)
            {
                var result = await _agentHttpClient.Get<ServiceResult<Dictionary<string, AgentTaskStatus>>>(CommandName, address, port);

                return new ServiceResult<IEnumerable<AgentTaskStatus>> { Success = true, Result = result.Result.Select(s => s.Value) };
            }

            var queryContent = new DynamicWrapper { { "TaskId", taskId } };

            var serviceResult = await _agentHttpClient.Get<ServiceResult<AgentTaskStatus>>(CommandName, address, port, queryContent);

            return new ServiceResult<AgentTaskStatus> { Success = true, Result = serviceResult.Result };
        }
    }
}