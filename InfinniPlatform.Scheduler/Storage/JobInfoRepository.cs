using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.Scheduler.Storage
{
    internal class JobInfoRepository : IJobInfoRepository
    {
        public JobInfoRepository(ISystemDocumentStorageFactory documentStorageFactory)
        {
            _jobInfoStorage = documentStorageFactory.GetStorage<JobInfo>();
        }


        private readonly ISystemDocumentStorage<JobInfo> _jobInfoStorage;


        public async Task<IEnumerable<IJobInfo>> GetActualJobInfos()
        {
            return await _jobInfoStorage.Find().ToListAsync();
        }

        public async Task<IEnumerable<string>> GetJobIds(Regex group = null)
        {
            var jobIds = await ((group != null)
                ? _jobInfoStorage.Find(i => group.IsMatch(i.Group)).Project(i => i._id).ToListAsync()
                : _jobInfoStorage.Find().Project(i => i._id).ToListAsync());

            return jobIds.Cast<string>();
        }

        public async Task<IJobInfo> GetJobInfo(string jobId)
        {
            var jobIdObj = EnsureJobId(jobId);

            return await _jobInfoStorage.Find(i => i._id == jobIdObj).FirstOrDefaultAsync();
        }


        public Task AddOrUpdateJob(IJobInfo jobInfo)
        {
            var jobInfoImpl = EnsureJobInfo(jobInfo);

            return _jobInfoStorage.SaveOneAsync(jobInfoImpl);
        }

        public Task AddOrUpdateJobs(IEnumerable<IJobInfo> jobInfos)
        {
            var jobInfoImpls = EnsureJobInfos(jobInfos).ToList();

            return _jobInfoStorage.SaveManyAsync(jobInfoImpls);
        }


        public Task DeleteJob(string jobId)
        {
            var jobIdObj = EnsureJobId(jobId);

            return _jobInfoStorage.DeleteOneAsync(i => i._id == jobIdObj);
        }

        public Task DeleteJobs(IEnumerable<string> jobIds)
        {
            var jobIdObjs = EnsureJobIds(jobIds).ToList();

            return _jobInfoStorage.DeleteOneAsync(i => jobIdObjs.Contains(i._id));
        }

        public Task DeleteAllJobs()
        {
            return _jobInfoStorage.DeleteManyAsync();
        }


        public Task PauseJob(string jobId)
        {
            var jobIdObj = EnsureJobId(jobId);

            return _jobInfoStorage.UpdateOneAsync(u => u.Set(i => i.State, JobState.Paused), i => i._id == jobIdObj && i.State != JobState.Paused);
        }

        public Task PauseJobs(IEnumerable<string> jobIds)
        {
            var jobIdObjs = EnsureJobIds(jobIds).ToList();

            return _jobInfoStorage.UpdateOneAsync(u => u.Set(i => i.State, JobState.Paused), i => jobIdObjs.Contains(i._id) && i.State != JobState.Paused);
        }

        public Task PauseAllJobs()
        {
            return _jobInfoStorage.UpdateOneAsync(u => u.Set(i => i.State, JobState.Paused), i => i.State != JobState.Paused);
        }


        public Task ResumeJob(string jobId)
        {
            var jobIdObj = EnsureJobId(jobId);

            return _jobInfoStorage.UpdateOneAsync(u => u.Set(i => i.State, JobState.Planned), i => i._id == jobIdObj && i.State != JobState.Planned);
        }

        public Task ResumeJobs(IEnumerable<string> jobIds)
        {
            var jobIdObjs = EnsureJobIds(jobIds).ToList();

            return _jobInfoStorage.UpdateOneAsync(u => u.Set(i => i.State, JobState.Planned), i => jobIdObjs.Contains(i._id) && i.State != JobState.Planned);
        }

        public Task ResumeAllJobs()
        {
            return _jobInfoStorage.UpdateOneAsync(u => u.Set(i => i.State, JobState.Planned), i => i.State != JobState.Planned);
        }


        private JobInfo EnsureJobInfo(IJobInfo jobInfo)
        {
            if (jobInfo == null)
            {
                throw new ArgumentNullException(nameof(jobInfo));
            }

            var jobInfoImpl = jobInfo as JobInfo;

            if (jobInfoImpl == null)
            {
                throw new ArgumentException(nameof(jobInfo));
            }

            return jobInfoImpl;
        }

        private IEnumerable<JobInfo> EnsureJobInfos(IEnumerable<IJobInfo> jobInfos)
        {
            if (jobInfos == null)
            {
                throw new ArgumentNullException(nameof(jobInfos));
            }

            return jobInfos.Select(EnsureJobInfo);
        }


        private object EnsureJobId(string jobId)
        {
            if (string.IsNullOrEmpty(jobId))
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            return jobId;
        }

        private IEnumerable<object> EnsureJobIds(IEnumerable<string> jobIds)
        {
            if (jobIds == null)
            {
                throw new ArgumentNullException(nameof(jobIds));
            }

            return jobIds.Select(EnsureJobId);
        }
    }
}