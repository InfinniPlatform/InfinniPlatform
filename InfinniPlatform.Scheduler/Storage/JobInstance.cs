using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Scheduler.Storage
{
    [DocumentType(SchedulerConstants.ObjectNamePrefix + nameof(JobInstance))]
    internal class JobInstance : Document
    {
    }
}