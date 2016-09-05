using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Sdk.Diagnostics;

namespace InfinniPlatform.Scheduler.Diagnostics
{
    /// <summary>
    /// Предоставляет информацию о состоянии подсистемы планирования заданий.
    /// </summary>
    internal class SchedulerStatusProvider : ISubsystemStatusProvider
    {
        public SchedulerStatusProvider(IJobScheduler jobScheduler)
        {
            _jobScheduler = jobScheduler;
        }


        private readonly IJobScheduler _jobScheduler;


        public string Name => "scheduler";


        public async Task<object> GetStatus()
        {
            return await _jobScheduler.GetStatus();
        }
    }
}