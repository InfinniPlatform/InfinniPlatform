using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinniPlatform.Scheduler.Repository
{
    /// <summary>
    /// Источник сохраненных заданий.
    /// </summary>
    internal class PersistentJobInfoSource : IJobInfoSource
    {
        public PersistentJobInfoSource(IJobSchedulerRepository jobSchedulerRepository)
        {
            _jobSchedulerRepository = jobSchedulerRepository;
        }


        private readonly IJobSchedulerRepository _jobSchedulerRepository;


        public async Task<IEnumerable<IJobInfo>> GetJobs(IJobInfoFactory factory)
        {
            return await _jobSchedulerRepository.GetActualJobInfos();
        }
    }
}