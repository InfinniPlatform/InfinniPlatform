using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Scheduler.Implementation;

namespace InfinniPlatform.Scheduler.Storage
{
    /// <summary>
    /// Источник заданий хранилища <see cref="IJobInfoRepository"/>.
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