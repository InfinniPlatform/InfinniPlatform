using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Scheduler.Clusterization
{
    internal class JobSchedulerStateObserverStub : IJobSchedulerStateObserver
    {
        public JobSchedulerStateObserverStub(JobHandlerConsumer jobHandlerConsumer)
        {
            _jobHandlerConsumer = jobHandlerConsumer;
        }


        private readonly IConsumer _jobHandlerConsumer;


        public Task OnAddOrUpdateJob(JobInfo jobInfo)
        {
            return Task.CompletedTask;
        }

        public Task OnAddOrUpdateJobs(IEnumerable<JobInfo> jobInfos)
        {
            return Task.CompletedTask;
        }


        public Task OnDeleteJob(string jobId)
        {
            return Task.CompletedTask;
        }

        public Task OnDeleteJobs(IEnumerable<string> jobIds)
        {
            return Task.CompletedTask;
        }

        public Task OnDeleteAllJobs()
        {
            return Task.CompletedTask;
        }


        public Task OnPauseJob(string jobId)
        {
            return Task.CompletedTask;
        }

        public Task OnPauseJobs(IEnumerable<string> jobIds)
        {
            return Task.CompletedTask;
        }

        public Task OnPauseAllJobs()
        {
            return Task.CompletedTask;
        }


        public Task OnResumeJob(string jobId)
        {
            return Task.CompletedTask;
        }

        public Task OnResumeJobs(IEnumerable<string> jobIds)
        {
            return Task.CompletedTask;
        }

        public Task OnResumeAllJobs()
        {
            return Task.CompletedTask;
        }


        public Task OnExecuteJob(JobInfo jobInfo, JobHandlerContext jobHandlerContext)
        {
            return _jobHandlerConsumer.Consume(new Message<JobHandlerEvent>(new JobHandlerEvent { JobInfo = jobInfo, Context = jobHandlerContext }));
        }
    }
}