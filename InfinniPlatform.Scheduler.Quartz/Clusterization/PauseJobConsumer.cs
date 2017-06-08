using System;
using System.Threading.Tasks;

using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue;
using InfinniPlatform.Scheduler.Properties;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Scheduler.Clusterization
{
    /// <summary>
    /// Обработчик события <see cref="PauseJobEvent" />.
    /// </summary>
    [LoggerName(nameof(PauseJobConsumer))]
    internal class PauseJobConsumer : BroadcastConsumerBase<PauseJobEvent>
    {
        public PauseJobConsumer(IJobSchedulerDispatcher jobSchedulerDispatcher,
                                AppOptions appOptions,
                                IPerformanceLogger<PauseJobConsumer> perfLogger,
                                ILogger<PauseJobConsumer> logger)
        {
            _jobSchedulerDispatcher = jobSchedulerDispatcher;
            _appOptions = appOptions;
            _perfLogger = perfLogger;
            _logger = logger;
        }


        private readonly IJobSchedulerDispatcher _jobSchedulerDispatcher;
        private readonly AppOptions _appOptions;
        private readonly IPerformanceLogger _perfLogger;
        private readonly ILogger _logger;


        protected override async Task Consume(Message<PauseJobEvent> message)
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
                    await _jobSchedulerDispatcher.PauseAllJobs();
                }
                else if (message.Body.JobIds != null)
                {
                    await _jobSchedulerDispatcher.PauseJobs(message.Body.JobIds);
                }
            }
            catch (Exception exception)
            {
                error = exception;

                _logger.LogError(Resources.PauseJobsCompletedWithException, exception);
            }
            finally
            {
                _perfLogger.Log(nameof(DeleteJobConsumer), startTime, error);
            }
        }
    }
}