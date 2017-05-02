using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Scheduler.Clusterization
{
    internal class JobSchedulerStateObserver : IJobSchedulerStateObserver
    {
        public JobSchedulerStateObserver(IBroadcastProducer broadcastProducer, ITaskProducer taskProducer)
        {
            _broadcastProducer = broadcastProducer;
            _taskProducer = taskProducer;
        }


        private readonly IBroadcastProducer _broadcastProducer;
        private readonly ITaskProducer _taskProducer;


        public Task OnAddOrUpdateJob(JobInfo jobInfo)
        {
            return _broadcastProducer.PublishAsync(new AddOrUpdateJobEvent { JobInfos = new[] { jobInfo } });
        }

        public Task OnAddOrUpdateJobs(IEnumerable<JobInfo> jobInfos)
        {
            return _broadcastProducer.PublishAsync(new AddOrUpdateJobEvent { JobInfos = jobInfos });
        }


        public Task OnDeleteJob(string jobId)
        {
            return _broadcastProducer.PublishAsync(new DeleteJobEvent { JobIds = new[] { jobId } });
        }

        public Task OnDeleteJobs(IEnumerable<string> jobIds)
        {
            return _broadcastProducer.PublishAsync(new DeleteJobEvent { JobIds = jobIds });
        }

        public Task OnDeleteAllJobs()
        {
            return _broadcastProducer.PublishAsync(new DeleteJobEvent { All = true });
        }


        public Task OnPauseJob(string jobId)
        {
            return _broadcastProducer.PublishAsync(new PauseJobEvent { JobIds = new[] { jobId } });
        }

        public Task OnPauseJobs(IEnumerable<string> jobIds)
        {
            return _broadcastProducer.PublishAsync(new PauseJobEvent { JobIds = jobIds });
        }

        public Task OnPauseAllJobs()
        {
            return _broadcastProducer.PublishAsync(new PauseJobEvent { All = true });
        }


        public Task OnResumeJob(string jobId)
        {
            return _broadcastProducer.PublishAsync(new ResumeJobEvent { JobIds = new[] { jobId } });
        }

        public Task OnResumeJobs(IEnumerable<string> jobIds)
        {
            return _broadcastProducer.PublishAsync(new ResumeJobEvent { JobIds = jobIds });
        }

        public Task OnResumeAllJobs()
        {
            return _broadcastProducer.PublishAsync(new ResumeJobEvent { All = true });
        }


        public Task OnExecuteJob(JobInfo jobInfo, JobHandlerContext jobHandlerContext)
        {
            return _taskProducer.PublishAsync(new JobHandlerEvent { JobInfo = jobInfo, Context = jobHandlerContext });
        }


        public static bool CanBeCreated(IContainerResolver resolver)
        {
            return resolver.IsRegistered<IBroadcastProducer>() && resolver.IsRegistered<ITaskProducer>();
        }
    }
}