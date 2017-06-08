using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;
using InfinniPlatform.Logging;
using InfinniPlatform.Scheduler.Properties;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Scheduler
{
    [LoggerName(nameof(JobScheduler))]
    internal class JobScheduler : IJobScheduler
    {
        public JobScheduler(IJobSchedulerDispatcher jobSchedulerDispatcher,
                            IJobSchedulerStateObserver jobSchedulerStateObserver,
                            IJobSchedulerRepository jobSchedulerRepository,
                            IPerformanceLogger<JobScheduler> perfLogger,
                            ILogger<JobScheduler> logger)
        {
            _jobSchedulerDispatcher = jobSchedulerDispatcher;
            _jobSchedulerRepository = jobSchedulerRepository;
            _jobSchedulerStateObserver = jobSchedulerStateObserver;
            _perfLogger = perfLogger;
            _logger = logger;
        }


        private readonly IJobSchedulerDispatcher _jobSchedulerDispatcher;
        private readonly IJobSchedulerRepository _jobSchedulerRepository;
        private readonly IJobSchedulerStateObserver _jobSchedulerStateObserver;
        private readonly IPerformanceLogger _perfLogger;
        private readonly ILogger _logger;


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
                                           await _jobSchedulerRepository.AddOrUpdateJob(jobInfoImpl);
                                           await _jobSchedulerStateObserver.OnAddOrUpdateJob(jobInfoImpl);
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
                                               await _jobSchedulerRepository.AddOrUpdateJobs(jobInfoImpls);
                                               await _jobSchedulerStateObserver.OnAddOrUpdateJobs(jobInfoImpls);
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
                                           await _jobSchedulerRepository.DeleteJob(jobId);
                                           await _jobSchedulerStateObserver.OnDeleteJob(jobId);
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
                                               await _jobSchedulerRepository.DeleteJobs(jobIdList);
                                               await _jobSchedulerStateObserver.OnDeleteJobs(jobIdList);
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
                                           await _jobSchedulerRepository.DeleteAllJobs();
                                           await _jobSchedulerStateObserver.OnDeleteAllJobs();
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
                                           await _jobSchedulerRepository.PauseJob(jobId);
                                           await _jobSchedulerStateObserver.OnPauseJob(jobId);
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
                                               await _jobSchedulerRepository.PauseJobs(jobIdList);
                                               await _jobSchedulerStateObserver.OnPauseJobs(jobIdList);
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
                                           await _jobSchedulerRepository.PauseAllJobs();
                                           await _jobSchedulerStateObserver.OnPauseAllJobs();
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
                                           await _jobSchedulerRepository.ResumeJob(jobId);
                                           await _jobSchedulerStateObserver.OnResumeJob(jobId);
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
                                               await _jobSchedulerRepository.ResumeJobs(jobIdList);
                                               await _jobSchedulerStateObserver.OnResumeJobs(jobIdList);
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
                                           await _jobSchedulerRepository.ResumeAllJobs();
                                           await _jobSchedulerStateObserver.OnResumeAllJobs();
                                           return true;
                                       },
                                 nameof(ResumeAllJobs));
        }


        public Task TriggerJob(string jobId, DynamicDocument data = null)
        {
            return ExecuteAction(async () =>
                                       {
                                           CommonExtensions.EnsureJobId(jobId);
                                           await _jobSchedulerDispatcher.TriggerJob(jobId, data);
                                           return true;
                                       },
                                 nameof(TriggerJob));
        }

        public Task TriggerJobs(IEnumerable<string> jobIds, DynamicDocument data = null)
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

        public Task TriggerAllJob(DynamicDocument data = null)
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

                _logger.LogError(string.Format(Resources.JobSchedulerMethodCompletedWithException, logMethod), error);

                throw;
            }
            finally
            {
                _perfLogger.Log(logMethod, startTime, error);
            }
        }
    }
}