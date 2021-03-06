﻿using System;
using System.Threading.Tasks;

using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue;
using InfinniPlatform.Scheduler.Properties;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Scheduler.Clusterization
{
    /// <summary>
    /// Обработчик события <see cref="AddOrUpdateJobEvent" />.
    /// </summary>
    [LoggerName(nameof(AddOrUpdateJobConsumer))]
    internal class AddOrUpdateJobConsumer : BroadcastConsumerBase<AddOrUpdateJobEvent>
    {
        public AddOrUpdateJobConsumer(IJobSchedulerDispatcher jobSchedulerDispatcher,
                                      AppOptions appOptions,
                                      IPerformanceLogger<AddOrUpdateJobConsumer> perfLogger,
                                      ILogger<AddOrUpdateJobConsumer> logger)
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

                _logger.LogError(Resources.AddOrUpdateJobsCompletedWithException, exception);
            }
            finally
            {
                _perfLogger.Log(nameof(AddOrUpdateJobConsumer), startTime, error);
            }
        }
    }
}