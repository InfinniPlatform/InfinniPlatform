using InfinniPlatform.DocumentStorage;

namespace InfinniPlatform.Scheduler.Storage
{
    [DocumentType(SchedulerExtensions.ObjectNamePrefix + nameof(JobInstance))]
    internal class JobInstance : Document
    {
    }
}