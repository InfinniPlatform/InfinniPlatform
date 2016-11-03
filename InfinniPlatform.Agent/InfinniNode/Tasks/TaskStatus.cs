using System;

using InfinniPlatform.Agent.Helpers;

namespace InfinniPlatform.Agent.InfinniNode.Tasks
{
    public class TaskStatus
    {
        public TaskStatus(ProcessHelper.ProcessResult processResult)
        {
            ProcessResult = processResult;
            TaskId = Guid.NewGuid().ToString("D");
        }

        public string TaskId { get; set; }

        public string Log { get; set; }

        public ProcessHelper.ProcessResult ProcessResult { get; set; }
    }
}