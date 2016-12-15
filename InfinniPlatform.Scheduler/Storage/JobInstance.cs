using InfinniPlatform.DocumentStorage.Contract;
using InfinniPlatform.Scheduler.Contract;

namespace InfinniPlatform.Scheduler.Storage
{
    [DocumentType(SchedulerExtensions.ObjectNamePrefix + nameof(JobInstance))]
    internal class JobInstance : Document
    {
    }
}