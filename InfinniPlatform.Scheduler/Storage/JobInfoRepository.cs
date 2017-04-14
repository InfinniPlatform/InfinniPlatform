using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage.Abstractions;
using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Scheduler.Contract;

namespace InfinniPlatform.Scheduler.Storage
{
    internal class JobInfoRepository : IJobInfoRepository
    {
        private static readonly Task CompletedTask = Task.FromResult(true);


        public JobInfoRepository(ISystemDocumentStorageFactory documentStorageFactory)
        {
            _jobInfoStorage = documentStorageFactory.GetStorage<JobInfo>();
        }


        private readonly ISystemDocumentStorage<JobInfo> _jobInfoStorage;


        public async Task<IEnumerable<IJobInfo>> GetActualJobInfos()
        {
            return await _jobInfoStorage.Find().ToListAsync();
        }


        public Task AddOrUpdateJob(IJobInfo jobInfo)
        {
            var jobInfoImpl = CommonExtensions.EnsureJobInfo(jobInfo);

            return _jobInfoStorage.SaveOneAsync(jobInfoImpl);
        }

        public Task AddOrUpdateJobs(IEnumerable<IJobInfo> jobInfos)
        {
            var jobInfoImpls = CommonExtensions.EnsureJobInfos(jobInfos);

            return (jobInfoImpls.Count > 0)
                ? _jobInfoStorage.SaveManyAsync(jobInfoImpls)
                : CompletedTask;
        }


        public Task DeleteJob(string jobId)
        {
            object jobIdObj = CommonExtensions.EnsureJobId(jobId);

            return _jobInfoStorage.DeleteOneAsync(i => i._id == jobIdObj);
        }

        public Task DeleteJobs(IEnumerable<string> jobIds)
        {
            var jobIdObjs = CommonExtensions.EnsureJobIds(jobIds);

            return (jobIdObjs.Count > 0)
                ? _jobInfoStorage.DeleteOneAsync(i => jobIdObjs.Contains(i._id))
                : CompletedTask;
        }

        public Task DeleteAllJobs()
        {
            return _jobInfoStorage.DeleteManyAsync();
        }


        public Task PauseJob(string jobId)
        {
            object jobIdObj = CommonExtensions.EnsureJobId(jobId);

            return _jobInfoStorage.UpdateOneAsync(u => u.Set(i => i.State, JobState.Paused), i => i._id == jobIdObj && i.State != JobState.Paused);
        }

        public Task PauseJobs(IEnumerable<string> jobIds)
        {
            var jobIdObjs = CommonExtensions.EnsureJobIds(jobIds);

            return (jobIdObjs.Count > 0)
                ? _jobInfoStorage.UpdateOneAsync(u => u.Set(i => i.State, JobState.Paused), i => jobIdObjs.Contains(i._id) && i.State != JobState.Paused)
                : CompletedTask;
        }

        public Task PauseAllJobs()
        {
            return _jobInfoStorage.UpdateOneAsync(u => u.Set(i => i.State, JobState.Paused), i => i.State != JobState.Paused);
        }


        public Task ResumeJob(string jobId)
        {
            object jobIdObj = CommonExtensions.EnsureJobId(jobId);

            return _jobInfoStorage.UpdateOneAsync(u => u.Set(i => i.State, JobState.Planned), i => i._id == jobIdObj && i.State != JobState.Planned);
        }

        public Task ResumeJobs(IEnumerable<string> jobIds)
        {
            var jobIdObjs = CommonExtensions.EnsureJobIds(jobIds);

            return (jobIdObjs.Count > 0)
                ? _jobInfoStorage.UpdateOneAsync(u => u.Set(i => i.State, JobState.Planned), i => jobIdObjs.Contains(i._id) && i.State != JobState.Planned)
                : CompletedTask;
        }

        public Task ResumeAllJobs()
        {
            return _jobInfoStorage.UpdateOneAsync(u => u.Set(i => i.State, JobState.Planned), i => i.State != JobState.Planned);
        }
    }
}