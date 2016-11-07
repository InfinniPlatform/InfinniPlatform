using System;
using System.Collections.Generic;

namespace InfinniPlatform.Agent.Tasks
{
    public class NodeTaskStorage : INodeTaskStorage
    {
        public NodeTaskStorage()
        {
            _taskStorage = new Dictionary<string, TaskStatus>();
        }

        private readonly Dictionary<string, TaskStatus> _taskStorage;

        public string AddNewTask()
        {
            var taskStatus = new TaskStatus
                             {
                                 TaskId = Guid.NewGuid().ToString("D")
                             };

            _taskStorage.Add(taskStatus.TaskId, taskStatus);

            return taskStatus.TaskId;
        }

        public void RemoveTask(string taskId)
        {
            _taskStorage.Remove(taskId);
        }

        public TaskStatus GetTaskStatus(string taskId)
        {
            return _taskStorage[taskId];
        }

        public void AddOutput(string taskId, string output)
        {
            _taskStorage[taskId].Output += $"{output}{System.Environment.NewLine}";
        }

        public void SetCompleted(string taskId)
        {
            _taskStorage[taskId].Completed = true;
        }
    }
}