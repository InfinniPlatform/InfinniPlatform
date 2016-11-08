using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.Tasks.InfinniNode
{
    public class TaskStatusTask : IAgentTask
    {
        public TaskStatusTask(ITaskStorage taskStorage)
        {
            _taskStorage = taskStorage;
        }

        private readonly ITaskStorage _taskStorage;

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string CommandName => "taskStatus";

        public Task<object> Run(IHttpRequest request)
        {
            string taskId = request.Query.TaskId;

            if (taskId != null)
            {
                var result = _taskStorage.GetTaskStatus(taskId);

                var serviceResult = new ServiceResult<TaskStatus> { Success = true, Result = result };

                return Task.FromResult<object>(serviceResult);
            }
            else
            {
                var result = _taskStorage.GetTaskStatusStorage();

                var serviceResult = new ServiceResult<Dictionary<string, TaskStatus>> { Success = true, Result = result };

                return Task.FromResult<object>(serviceResult);
            }
        }
    }
}