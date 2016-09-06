using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Scheduler.Storage
{
    [DocumentType(SchedulerExtensions.ObjectNamePrefix + nameof(JobInstance))]
    internal class JobInstance : Document
    {
    }
}