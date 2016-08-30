using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Scheduler.Contract;

namespace InfinniPlatform.Scheduler.Storage
{
    /// <summary>
    /// Источник сохраненных заданий.
    /// </summary>
    internal class PersistentJobInfoSource : IJobInfoSource
    {
        public PersistentJobInfoSource(IJobInfoRepository jobInfoRepository)
        {
            _jobInfoRepository = jobInfoRepository;
        }


        private readonly IJobInfoRepository _jobInfoRepository;


        public Task<IEnumerable<IJobInfo>> GetJobs(IJobInfoFactory factory)
        {
            return _jobInfoRepository.GetPlannedJobInfos();
        }
    }
}