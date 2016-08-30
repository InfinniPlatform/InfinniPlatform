using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.Scheduler.Common
{
    /// <summary>
    /// Событие о необходимости обработать задание.
    /// </summary>
    [QueueName("Scheduler.HandleJob")]
    internal class HandleJobMessage
    {
        public JobInfo JobInfo { get; set; }

        public JobHandlerContext Context { get; set; }
    }
}