using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Server.Agent;

namespace InfinniPlatform.Server.RestApi
{
    public class TaskStatusHttpService : IHttpService
    {
        public TaskStatusHttpService(IAgentHttpClient agentHttpClient)
        {
            _agentHttpClient = agentHttpClient;
        }

        private readonly IAgentHttpClient _agentHttpClient;

        public void Load(IHttpServiceBuilder builder)
        {
            builder.ServicePath = "server";
            builder.Get["taskStatus"] = GetTaskStatus;
        }

        private async Task<object> GetTaskStatus(IHttpRequest request)
        {
            string address = request.Query.Address;
            int port = request.Query.Port;

            var taskId = (string)request.Query.TaskId;

            if (taskId == null)
            {
                var result = await _agentHttpClient.Get<ServiceResult<Dictionary<string, AgentTaskStatus>>>("taskStatus", address, port);

                var taskStatuses = result.Result.Select(pair => Convert(pair.Value));

                return new ServiceResult<IEnumerable<DynamicWrapper>> { Success = true, Result = taskStatuses };
            }

            var queryContent = new DynamicWrapper { { "TaskId", taskId } };

            var serviceResult = await _agentHttpClient.Get<ServiceResult<AgentTaskStatus>>("taskStatus", address, port, queryContent);

            return new ServiceResult<DynamicWrapper>
                   {
                       Success = true, Result = Convert(serviceResult.Result)
                   };
        }

        private DynamicWrapper Convert(AgentTaskStatus taskStatus)
        {
            return new DynamicWrapper
                   {
                       {
                           "Completed", taskStatus.Completed
                                            ? "Completed"
                                            : "Working"
                       },
                       { "Description", taskStatus.Description }
                   };
        }
    }
}