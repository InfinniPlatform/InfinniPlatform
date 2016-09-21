using System;
using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Scheduler.Properties;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Scheduler.Queues
{
    /// <summary>
    /// Обработчик события <see cref="AddOrUpdateJobEvent" />.
    /// </summary>
    internal class AddOrUpdateJobConsumer : BroadcastConsumerBase<AddOrUpdateJobEvent>
    {
        public AddOrUpdateJobConsumer(IJobSchedulerDispatcher jobSchedulerDispatcher,
                                      IAppEnvironment appEnvironment,
                                      IPerformanceLog performanceLog,
                                      ILog log)
        {
            _jobSchedulerDispatcher = jobSchedulerDispatcher;
            _appEnvironment = appEnvironment;
            _performanceLog = performanceLog;
            _log = log;
        }


        private readonly IJobSchedulerDispatcher _jobSchedulerDispatcher;
        private readonly IAppEnvironment _appEnvironment;
        private readonly IPerformanceLog _performanceLog;
        private readonly ILog _log;


        protected override async Task Consume(Message<AddOrUpdateJobEvent> message)
        {
            var startTime = DateTime.Now;

            Exception error = null;

            try
            {
                // События, посланные с одного и того же узла, не обрабатываются
                if (string.Equals(_appEnvironment.InstanceId, message.AppId, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                var jobInfos = message.Body.JobInfos;

                if (jobInfos != null)
                {
                    await _jobSchedulerDispatcher.AddOrUpdateJobs(jobInfos);
                }
            }
            catch (Exception exception)
            {
                error = exception;

                _log.Error(Resources.AddOrUpdateJobsCompletedWithException, exception);
            }
            finally
            {
                _performanceLog.Log(nameof(AddOrUpdateJobConsumer), startTime, error);
            }
        }
    }
}