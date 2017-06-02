using System;
using System.Threading.Tasks;

using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue;
using InfinniPlatform.Scheduler.Properties;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Scheduler.Clusterization
{
    /// <summary>
    /// Обработчик события <see cref="DeleteJobEvent" />.
    /// </summary>
    internal class DeleteJobConsumer : BroadcastConsumerBase<DeleteJobEvent>
    {
        public DeleteJobConsumer(IJobSchedulerDispatcher jobSchedulerDispatcher,
                                 AppOptions appOptions,
                                 IPerformanceLogger<DeleteJobConsumer> perfLogger,
                                 ILogger<DeleteJobConsumer> logger)
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


        protected override async Task Consume(Message<DeleteJobEvent> message)
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
                    await _jobSchedulerDispatcher.DeleteAllJobs();
                }
                else if (message.Body.JobIds != null)
                {
                    await _jobSchedulerDispatcher.DeleteJobs(message.Body.JobIds);
                }
            }
            catch (Exception exception)
            {
                error = exception;

                _logger.LogError(Resources.DeleteJobsCompletedWithException, exception);
            }
            finally
            {
                _perfLogger.Log(nameof(DeleteJobConsumer), startTime, error);
            }
        }
    }
}