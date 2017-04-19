using System;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;
using InfinniPlatform.Core.Abstractions.Logging;
using InfinniPlatform.Core.Abstractions.Settings;
using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Scheduler.Properties;

namespace InfinniPlatform.Scheduler.Queues
{
    /// <summary>
    /// Обработчик события <see cref="AddOrUpdateJobEvent" />.
    /// </summary>
    internal class AddOrUpdateJobConsumer : BroadcastConsumerBase<AddOrUpdateJobEvent>
    {
        public AddOrUpdateJobConsumer(IJobSchedulerDispatcher jobSchedulerDispatcher,
                                      AppOptions appOptions,
                                      IPerformanceLog performanceLog,
                                      ILog log)
        {
            _jobSchedulerDispatcher = jobSchedulerDispatcher;
            _appOptions = appOptions;
            _performanceLog = performanceLog;
            _log = log;
        }


        private readonly IJobSchedulerDispatcher _jobSchedulerDispatcher;
        private readonly AppOptions _appOptions;
        private readonly IPerformanceLog _performanceLog;
        private readonly ILog _log;


        protected override async Task Consume(Message<AddOrUpdateJobEvent> message)
        {
            var startTime = DateTime.Now;

            Exception error = null;

            try
            {
                // События, посланные с одного и того же узла, не обрабатываются
                if (string.Equals(_appOptions.AppInstance, message.AppId, StringComparison.OrdinalIgnoreCase))
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