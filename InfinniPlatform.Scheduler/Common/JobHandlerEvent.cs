using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.Scheduler.Common
{
    /// <summary>
    /// Событие необходимости обработки задания.
    /// </summary>
    [QueueName(SchedulerConstants.ObjectNamePrefix + nameof(JobHandlerEvent))]
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