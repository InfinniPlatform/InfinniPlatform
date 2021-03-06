﻿using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Scheduler.Clusterization
{
    /// <summary>
    /// Событие необходимости обработки задания.
    /// </summary>
    [QueueName(SchedulerExtensions.ObjectNamePrefix + nameof(JobHandlerEvent))]
    internal class JobHandlerEvent
    {
        /// <summary>
        /// Информация о задании.
        /// </summary>
        public JobInfo JobInfo { get; set; }

        /// <summary>
        /// Контекст обработки задания.
        /// </summary>
        public JobHandlerContext Context { get; set; }
    }
}