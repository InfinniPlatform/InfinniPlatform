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
    /// Обработчик события <see cref="ResumeJobEvent" />.
    /// </summary>
    internal class ResumeJobConsumer : BroadcastConsumerBase<ResumeJobEvent>
    {
        public ResumeJobConsumer(IJobSchedulerDispatcher jobSchedulerDispatcher,
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


        protected override async Task Consume(Message<ResumeJobEvent> message)
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

                if (message.Body.All)
                {
                    await _jobSchedulerDispatcher.ResumeAllJobs();
                }
                else if (message.Body.JobIds != null)
                {
                    await _jobSchedulerDispatcher.ResumeJobs(message.Body.JobIds);
                }
            }
            catch (Exception exception)
            {
                error = exception;

                _log.Error(Resources.ResumingJobsCompletedWithException, exception);
            }
            finally
            {
                _performanceLog.Log(nameof(DeleteJobConsumer), startTime, error);
            }
        }
    }
}