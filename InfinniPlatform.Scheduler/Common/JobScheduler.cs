using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.Abstractions.Producers;
using InfinniPlatform.Core.Abstractions.Dynamic;
using InfinniPlatform.Core.Abstractions.Logging;
using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Scheduler.Properties;

namespace InfinniPlatform.Scheduler.Common
{
    [LoggerName(SchedulerExtensions.ComponentName)]
    internal class JobScheduler : IJobScheduler
    {
        public JobScheduler(IJobSchedulerDispatcher jobSchedulerDispatcher,
                            IJobInfoRepository jobInfoRepository,
                            IBroadcastProducer broadcastProducer,
                            IPerformanceLog performanceLog,
                            ILog log)
        {
            _jobSchedulerDispatcher = jobSchedulerDispatcher;
            _jobInfoRepository = jobInfoRepository;
            _broadcastProducer = broadcastProducer;
            _performanceLog = performanceLog;
            _log = log;
        }


        private readonly IJobSchedulerDispatcher _jobSchedulerDispatcher;
        private readonly IJobInfoRepository _jobInfoRepository;
        private readonly IBroadcastProducer _broadcastProducer;
        private readonly IPerformanceLog _performanceLog;
        private readonly ILog _log;


        public Task<bool> IsStarted()
        {
            return ExecuteAction(() => _jobSchedulerDispatcher.IsStarted(), nameof(IsStarted));
        }


        public Task<TResult> GetStatus<TResult>(Func<IEnumerable<IJobStatus>, TResult> selector)
        {
            return ExecuteAction(() => _jobSchedulerDispatcher.GetStatus(selector), nameof(GetStatus));
        }


        public Task AddOrUpdateJob(IJobInfo jobInfo)
        {
            return ExecuteAction(async () =>
                                       {
                                           var jobInfoImpl = CommonExtensions.EnsureJobInfo(jobInfo);
                                           await _jobSchedulerDispatcher.AddOrUpdateJob(jobInfoImpl);
                                           await _jobInfoRepository.AddOrUpdateJob(jobInfoImpl);
                                           await _broadcastProducer.PublishAsync(new AddOrUpdateJobEvent { JobInfos = new[] { jobInfoImpl } });
                                           return true;
                                       },
                                 nameof(AddOrUpdateJob));
        }

        public Task AddOrUpdateJobs(IEnumerable<IJobInfo> jobInfos)
        {
            return ExecuteAction(async () =>
                                       {
                                           var jobInfoImpls = CommonExtensions.EnsureJobInfos(jobInfos);

                                           if (jobInfoImpls.Count > 0)
                                           {
                                               await _jobSchedulerDispatcher.AddOrUpdateJobs(jobInfoImpls);
                                               await _jobInfoRepository.AddOrUpdateJobs(jobInfoImpls);
                                               await _broadcastProducer.PublishAsync(new AddOrUpdateJobEvent { JobInfos = jobInfoImpls });
                                           }

                                           return true;
                                       },
                                 nameof(AddOrUpdateJobs));
        }


        public Task DeleteJob(string jobId)
        {
            return ExecuteAction(async () =>
                                       {
                                           CommonExtensions.EnsureJobId(jobId);
                                           await _jobSchedulerDispatcher.DeleteJob(jobId);
                                           await _jobInfoRepository.DeleteJob(jobId);
                                           await _broadcastProducer.PublishAsync(new DeleteJobEvent { JobIds = new[] { jobId } });
                                           return true;
                                       },
                                 nameof(DeleteJob));
        }

        public Task DeleteJobs(IEnumerable<string> jobIds)
        {
            return ExecuteAction(async () =>
                                       {
                                           var jobIdList = CommonExtensions.EnsureJobIds(jobIds);

                                           if (jobIdList.Count > 0)
                                           {
                                               await _jobSchedulerDispatcher.DeleteJobs(jobIdList);
                                               await _jobInfoRepository.DeleteJobs(jobIdList);
                                               await _broadcastProducer.PublishAsync(new DeleteJobEvent { JobIds = jobIdList });
                                           }

                                           return true;
                                       },
                                 nameof(DeleteJobs));
        }

        public Task DeleteAllJobs()
        {
            return ExecuteAction(async () =>
                                       {
                                           await _jobSchedulerDispatcher.DeleteAllJobs();
                                           await _jobInfoRepository.DeleteAllJobs();
                                           await _broadcastProducer.PublishAsync(new DeleteJobEvent { All = true });
                                           return true;
                                       },
                                 nameof(DeleteAllJobs));
        }


        public Task PauseJob(string jobId)
        {
            return ExecuteAction(async () =>
                                       {
                                           CommonExtensions.EnsureJobId(jobId);
                                           await _jobSchedulerDispatcher.PauseJob(jobId);
                                           await _jobInfoRepository.PauseJob(jobId);
                                           await _broadcastProducer.PublishAsync(new PauseJobEvent { JobIds = new[] { jobId } });
                                           return true;
                                       },
                                 nameof(PauseJob));
        }

        public Task PauseJobs(IEnumerable<string> jobIds)
        {
            return ExecuteAction(async () =>
                                       {
                                           var jobIdList = CommonExtensions.EnsureJobIds(jobIds);

                                           if (jobIdList.Count > 0)
                                           {
                                               await _jobSchedulerDispatcher.PauseJobs(jobIdList);
                                               await _jobInfoRepository.PauseJobs(jobIdList);
                                               await _broadcastProducer.PublishAsync(new PauseJobEvent { JobIds = jobIdList });
                                           }

                                           return true;
                                       },
                                 nameof(PauseJobs));
        }

        public Task PauseAllJobs()
        {
            return ExecuteAction(async () =>
                                       {
                                           await _jobSchedulerDispatcher.PauseAllJobs();
                                           await _jobInfoRepository.PauseAllJobs();
                                           await _broadcastProducer.PublishAsync(new PauseJobEvent { All = true });
                                           return true;
                                       },
                                 nameof(PauseAllJobs));
        }


        public Task ResumeJob(string jobId)
        {
            return ExecuteAction(async () =>
                                       {
                                           CommonExtensions.EnsureJobId(jobId);
                                           await _jobSchedulerDispatcher.ResumeJob(jobId);
                                           await _jobInfoRepository.ResumeJob(jobId);
                                           await _broadcastProducer.PublishAsync(new ResumeJobEvent { JobIds = new[] { jobId } });
                                           return true;
                                       },
                                 nameof(ResumeJob));
        }

        public Task ResumeJobs(IEnumerable<string> jobIds)
        {
            return ExecuteAction(async () =>
                                       {
                                           var jobIdList = CommonExtensions.EnsureJobIds(jobIds);

                                           if (jobIdList.Count > 0)
                                           {
                                               await _jobSchedulerDispatcher.ResumeJobs(jobIdList);
                                               await _jobInfoRepository.ResumeJobs(jobIdList);
                                               await _broadcastProducer.PublishAsync(new ResumeJobEvent { JobIds = jobIdList });
                                           }

                                           return true;
                                       },
                                 nameof(ResumeJobs));
        }

        public Task ResumeAllJobs()
        {
            return ExecuteAction(async () =>
                                       {
                                           await _jobSchedulerDispatcher.ResumeAllJobs();
                                           await _jobInfoRepository.ResumeAllJobs();
                                           await _broadcastProducer.PublishAsync(new ResumeJobEvent { All = true });
                                           return true;
                                       },
                                 nameof(ResumeAllJobs));
        }


        public Task TriggerJob(string jobId, DynamicWrapper data = null)
        {
            return ExecuteAction(async () =>
                                       {
                                           CommonExtensions.EnsureJobId(jobId);
                                           await _jobSchedulerDispatcher.TriggerJob(jobId, data);
                                           return true;
                                       },
                                 nameof(TriggerJob));
        }

        public Task TriggerJobs(IEnumerable<string> jobIds, DynamicWrapper data = null)
        {
            return ExecuteAction(async () =>
                                       {
                                           var jobIdList = CommonExtensions.EnsureJobIds(jobIds);

                                           if (jobIdList.Count > 0)
                                           {
                                               await _jobSchedulerDispatcher.TriggerJobs(jobIdList, data);
                                           }

                                           return true;
                                       },
                                 nameof(TriggerJobs));
        }

        public Task TriggerAllJob(DynamicWrapper data = null)
        {
            return ExecuteAction(async () =>
                                       {
                                           await _jobSchedulerDispatcher.TriggerAllJob(data);
                                           return true;
                                       },
                                 nameof(TriggerAllJob));
        }


        private async Task<TResult> ExecuteAction<TResult>(Func<Task<TResult>> method, string logMethod)
        {
            var startTime = DateTime.Now;

            Exception error = null;

            try
            {
                return await method();
            }
            catch (Exception exception)
            {
                error = exception;

                _log.Error(string.Format(Resources.JobSchedulerMethodCompletedWithException, logMethod), error);

                throw;
            }
            finally
            {
                _performanceLog.Log(logMethod, startTime, error);
            }
        }
    }
}