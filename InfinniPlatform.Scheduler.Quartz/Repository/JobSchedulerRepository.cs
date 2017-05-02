using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.DocumentStorage;
using InfinniPlatform.IoC;

namespace InfinniPlatform.Scheduler.Repository
{
    internal class JobSchedulerRepository : IJobSchedulerRepository
    {
        public JobSchedulerRepository(ISystemDocumentStorageFactory documentStorageFactory)
        {
            _jobInfoStorage = documentStorageFactory.GetStorage<JobInfo>();
            _jobInstanceStorage = documentStorageFactory.GetStorage<JobInstance>();
        }


        private readonly ISystemDocumentStorage<JobInfo> _jobInfoStorage;
        private readonly ISystemDocumentStorage<JobInstance> _jobInstanceStorage;


        public async Task<IEnumerable<JobInfo>> GetActualJobInfos()
        {
            return await _jobInfoStorage.Find().ToListAsync();
        }


        public Task AddOrUpdateJob(JobInfo jobInfo)
        {
            var jobInfoImpl = CommonExtensions.EnsureJobInfo(jobInfo);

            return _jobInfoStorage.SaveOneAsync(jobInfoImpl);
        }

        public Task AddOrUpdateJobs(IEnumerable<JobInfo> jobInfos)
        {
            var jobInfoImpls = CommonExtensions.EnsureJobInfos(jobInfos);

            return (jobInfoImpls.Count > 0)
                ? _jobInfoStorage.SaveManyAsync(jobInfoImpls)
                : Task.CompletedTask;
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
                : Task.CompletedTask;
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
                : Task.CompletedTask;
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
                : Task.CompletedTask;
        }

        public Task ResumeAllJobs()
        {
            return _jobInfoStorage.UpdateOneAsync(u => u.Set(i => i.State, JobState.Planned), i => i.State != JobState.Planned);
        }


        public async Task<bool> IsHandledJob(string jobInstance)
        {
            if (string.IsNullOrEmpty(jobInstance))
            {
                throw new ArgumentNullException(nameof(jobInstance));
            }

            var jobLock = new JobInstance { _id = jobInstance };

            var result = await _jobInstanceStorage.SaveOneAsync(jobLock);

            return (result.UpdateStatus != DocumentUpdateStatus.Inserted);
        }


        public static bool CanBeCreated(IContainerResolver resolver)
        {
            return resolver.IsRegistered<ISystemDocumentStorageFactory>();
        }
    }
}