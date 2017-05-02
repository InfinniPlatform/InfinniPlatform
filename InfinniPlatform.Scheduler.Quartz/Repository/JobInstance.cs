using InfinniPlatform.DocumentStorage;

namespace InfinniPlatform.Scheduler.Repository
{
    [DocumentType(SchedulerExtensions.ObjectNamePrefix + nameof(JobInstance))]
    internal class JobInstance : Document
    {
    }
}