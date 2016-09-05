using InfinniPlatform.Scheduler.Contract;

namespace InfinniPlatform.Scheduler.Common
{
    internal class JobSchedulerStatus : IJobSchedulerStatus
    {
        public bool IsStarted { get; set; }

        public int Total { get; set; }

        public int Planned { get; set; }

        public int Paused { get; set; }
    }
}