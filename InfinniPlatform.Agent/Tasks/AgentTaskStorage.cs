using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.Caching;

using InfinniPlatform.Agent.Settings;

namespace InfinniPlatform.Agent.Tasks
{
    public class AgentTaskStorage : IAgentTaskStorage
    {
        public AgentTaskStorage(AgentSettings setting)
        {
            _setting = setting;
            _storage = new MemoryCache("Tasks");
            _dictionary = new ConcurrentDictionary<string, TaskStatus>();
        }

        private readonly ConcurrentDictionary<string, TaskStatus> _dictionary;
        private readonly AgentSettings _setting;

        private readonly MemoryCache _storage;

        public string AddNewTask(string description = null)
        {
            var taskStatus = new TaskStatus
                             {
                                 TaskId = Guid.NewGuid().ToString("D"),
                                 Description = description
                             };

            Set(taskStatus.TaskId, taskStatus);

            return taskStatus.TaskId;
        }

        public void RemoveTask(string taskId)
        {
            Remove(taskId);
        }

        public TaskStatus GetTaskStatus(string taskId)
        {
            return _dictionary[taskId];
        }

        public void AddOutput(string taskId, string output)
        {
            var task = _dictionary[taskId];
            task.Output += output;
        }

        public void SetCompleted(string taskId)
        {
            var task = _dictionary[taskId];
            task.Completed = true;
        }

        public IDictionary<string, TaskStatus> GetTaskStatusStorage()
        {
            return _dictionary;
        }

        private void Set(string taskId, TaskStatus task)
        {
            _dictionary.GetOrAdd(taskId, task);
            _storage.Set(taskId, taskId, new CacheItemPolicy
                                         {
                                             AbsoluteExpiration = DateTimeOffset.Now.Add(TimeSpan.FromMinutes(_setting.CacheTimeout)),
                                             RemovedCallback = arguments =>
                                                               {
                                                                   TaskStatus taskStatus;
                                                                   _dictionary.TryRemove(arguments.CacheItem.Key, out taskStatus);
                                                               }
                                         });
        }

        private void Remove(string taskId)
        {
            _storage.Remove(taskId);
            TaskStatus taskStatus;
            _dictionary.TryRemove(taskId, out taskStatus);
        }
    }
}