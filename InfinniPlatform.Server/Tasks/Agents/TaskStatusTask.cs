using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Server.Agent;

namespace InfinniPlatform.Server.Tasks.Agents
{
    public class TaskStatusTask : IServerTask
    {
        public TaskStatusTask(IAgentHttpClient agentHttpClient)
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

                var taskStatuses = result.Result.Select(pair => Convert(pair.Value));

                return new ServiceResult<IEnumerable<DynamicWrapper>> { Success = true, Result = taskStatuses };
            }

            var queryContent = new DynamicWrapper { { "TaskId", taskId } };

            var serviceResult = await _agentHttpClient.Get<ServiceResult<AgentTaskStatus>>(CommandName, address, port, queryContent);

            return new ServiceResult<DynamicWrapper>
                   {
                       Success = true,
                       Result = Convert(serviceResult.Result)
                   };
        }

        private static DynamicWrapper Convert(AgentTaskStatus taskStatus)
        {
            return new DynamicWrapper
                   {
                       {
                           "Completed", taskStatus.Completed
                                            ? "Completed"
                                            : "Working"
                       },
                       { "Description", taskStatus.Description },
                       { "Output", taskStatus.Output }
                   };
        }
    }
}