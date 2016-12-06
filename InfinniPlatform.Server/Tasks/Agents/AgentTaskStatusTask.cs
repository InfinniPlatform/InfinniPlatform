using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Server.Agent;

namespace InfinniPlatform.Server.Tasks.Agents
{
    /// <summary>
    /// Возвращает статус задачи, выполняемой на агенте.
    /// </summary>
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

            if (taskId != null)
            {
                return await GetTaskStatus(taskId, address, port);
            }

            return await GetTasksStatus(address, port);
        }

        private async Task<object> GetTaskStatus(string taskId, string address, int port)
        {
            var queryContent = new DynamicWrapper { { "TaskId", taskId } };

            var agentResult = await _agentHttpClient.Get<ServiceResult<AgentTaskStatus>>(CommandName, address, port, queryContent);

            var result = new ServiceResult<AgentTaskStatus> { Success = true, Result = agentResult.Result };

            return result;
        }

        private async Task<object> GetTasksStatus(string address, int port)
        {
            var agentResult = await _agentHttpClient.Get<ServiceResult<Dictionary<string, AgentTaskStatus>>>(CommandName, address, port);

            var result = new ServiceResult<IEnumerable<AgentTaskStatus>> { Success = true, Result = agentResult.Result.Select(s => s.Value) };

            return result;
        }
    }
}