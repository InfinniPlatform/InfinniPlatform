using System.Net.Http;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Http.Services;

namespace InfinniPlatform.Agent.Tasks.InfinniNode
{
    public class TaskStatusTask : IAppTask
    {
        public TaskStatusTask(INodeTaskStorage nodeTaskStorage)
        {
            _nodeTaskStorage = nodeTaskStorage;
        }

        private readonly INodeTaskStorage _nodeTaskStorage;

        public HttpMethod HttpMethod => HttpMethod.Get;

        public string CommandName => "taskStatus";

        public Task<object> Run(IHttpRequest request)
        {
            string taskId = request.Query.TaskId;

            var taskStatus = _nodeTaskStorage.GetTaskStatus(taskId);

            return Task.FromResult<object>(taskStatus);
        }
    }
}