using System.Collections.Generic;

using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Scheduler.Clusterization
{
    /// <summary>
    /// Событие добавления или обновления заданий.
    /// </summary>
    [QueueName(SchedulerExtensions.ObjectNamePrefix + nameof(AddOrUpdateJobEvent))]
    internal class AddOrUpdateJobEvent
    {
        /// <summary>
        /// Список с информацией о заданиях, которые необходимо добавить или удалить.
        /// </summary>
        public IEnumerable<JobInfo> JobInfos { get; set; }
    }
}