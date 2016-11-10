using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.Tasks.InfinniNode
{
    public class TaskStatusTask : IAgentTask
    {
        public TaskStatusTask(IAgentTaskStorage agentTaskStorage)
        {
            _agentTaskStorage = agentTaskStorage;
        }

        private readonly IAgentTaskStorage _agentTaskStorage;

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string CommandName => "taskStatus";

        public Task<object> Run(IHttpRequest request)
        {
            string taskId = request.Query.TaskId;

            if (taskId != null)
            {
                var result = _agentTaskStorage.GetTaskStatus(taskId);

                var serviceResult = new ServiceResult<TaskStatus> { Success = true, Result = result };

                return Task.FromResult<object>(serviceResult);
            }
            else
            {
                var result = _agentTaskStorage.GetTaskStatusStorage();

                var serviceResult = new ServiceResult<IDictionary<string, TaskStatus>> { Success = true, Result = result };

                return Task.FromResult<object>(serviceResult);
            }
        }
    }
}