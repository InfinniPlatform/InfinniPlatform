using System;
using System.Threading.Tasks;

using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue;
using InfinniPlatform.Scheduler.Properties;

namespace InfinniPlatform.Scheduler.Clusterization
{
    /// <summary>
    /// Обработчик события <see cref="ResumeJobEvent" />.
    /// </summary>
    internal class ResumeJobConsumer : BroadcastConsumerBase<ResumeJobEvent>
    {
        public ResumeJobConsumer(IJobSchedulerDispatcher jobSchedulerDispatcher,
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


        protected override async Task Consume(Message<ResumeJobEvent> message)
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