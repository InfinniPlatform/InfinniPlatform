using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.Scheduler.Common
{
    /// <summary>
    /// Событие о необходимости обработать задание.
    /// </summary>
    [QueueName(SchedulerConstants.ObjectNamePrefix + nameof(JobHandlerEvent))]
    internal class JobHandlerEvent
    {
        public JobInfo JobInfo { get; set; }

        public JobHandlerContext Context { get; set; }
    }
}