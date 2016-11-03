using System.Collections.Generic;

namespace InfinniPlatform.Agent.InfinniNode.Tasks
{
    public class TaskStorage : ITaskStorage
    {
        public TaskStorage()
        {
            _taskStorage = new Dictionary<string, TaskStatus>();
        }

        private readonly Dictionary<string, TaskStatus> _taskStorage;

        public void AddTask(TaskStatus taskStatus)
        {
            _taskStorage.Add(taskStatus.TaskId, taskStatus);
        }

        public void RemoveTask(string taskId)
        {
            _taskStorage.Remove(taskId);
        }

        public TaskStatus GetTaskStatus(string taskId)
        {
            return _taskStorage[taskId];
        }
    }


    public interface ITaskStorage
    {
        void AddTask(TaskStatus taskStatus);

        void RemoveTask(string taskId);

        TaskStatus GetTaskStatus(string taskId);
    }
}