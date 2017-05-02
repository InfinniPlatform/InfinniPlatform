using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinniPlatform.Scheduler.Repository
{
    internal class JobSchedulerRepositoryStub : IJobSchedulerRepository
    {
        public Task<IEnumerable<JobInfo>> GetActualJobInfos()
        {
            return Task.FromResult(Enumerable.Empty<JobInfo>());
        }


        public Task AddOrUpdateJob(JobInfo jobInfo)
        {
            return Task.CompletedTask;
        }

        public Task AddOrUpdateJobs(IEnumerable<JobInfo> jobInfos)
        {
            return Task.CompletedTask;
        }


        public Task DeleteJob(string jobId)
        {
            return Task.CompletedTask;
        }

        public Task DeleteJobs(IEnumerable<string> jobIds)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAllJobs()
        {
            return Task.CompletedTask;
        }


        public Task PauseJob(string jobId)
        {
            return Task.CompletedTask;
        }

        public Task PauseJobs(IEnumerable<string> jobIds)
        {
            return Task.CompletedTask;
        }

        public Task PauseAllJobs()
        {
            return Task.CompletedTask;
        }


        public Task ResumeJob(string jobId)
        {
            return Task.CompletedTask;
        }

        public Task ResumeJobs(IEnumerable<string> jobIds)
        {
            return Task.CompletedTask;
        }

        public Task ResumeAllJobs()
        {
            return Task.CompletedTask;
        }


        public Task<bool> IsHandledJob(string jobInstance)
        {
            return Task.FromResult(false);
        }
    }
}