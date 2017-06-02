using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Logging;
using InfinniPlatform.MessageQueue;
using InfinniPlatform.Scheduler.Properties;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Scheduler.Clusterization
{
    /// <summary>
    /// Обработчик события <see cref="JobHandlerEvent"/>.
    /// </summary>
    internal class JobHandlerConsumer : TaskConsumerBase<JobHandlerEvent>
    {
        public JobHandlerConsumer(IJobHandlerTypeSerializer jobHandlerTypeSerializer,
                                  IPerformanceLogger<JobHandlerConsumer> perfLogger,
                                  ILogger<JobHandlerConsumer> logger)
        {
            _jobHandlerTypeSerializer = jobHandlerTypeSerializer;
            _perfLogger = perfLogger;
            _logger = logger;
        }


        private readonly IJobHandlerTypeSerializer _jobHandlerTypeSerializer;
        private readonly IPerformanceLogger _perfLogger;
        private readonly ILogger _logger;


        protected override async Task Consume(Message<JobHandlerEvent> message)
        {
            var startTime = DateTime.Now;

            Exception error = null;

            IJobInfo jobInfo = null;
            IJobHandlerContext context = null;

            try
            {
                // Информация о здании
                jobInfo = message.Body.JobInfo;
                context = message.Body.Context;

                // Имя типа обработчика заданий
                var jobHandlerType = jobInfo.HandlerType;

                // Создание экземпляра обработчика заданий
                var handler = _jobHandlerTypeSerializer.Deserialize(jobHandlerType);

                if (handler != null)
                {
                    // Обработка задания
                    await handler.Handle(jobInfo, context);
                }
            }
            catch (Exception exception)
            {
                error = exception;

                Func<Dictionary<string, object>> logContext = () => new Dictionary<string, object>
                                                                    {
                                                                        { "jobId", jobInfo?.Id },
                                                                        { "jobInstanceId", context?.InstanceId },
                                                                        { "jobHandlerType", jobInfo?.HandlerType }
                                                                    };

                _logger.LogError(Resources.HandlingOfJobCompletedWithException, exception, logContext);
            }
            finally
            {
                _perfLogger.Log(nameof(JobHandlerConsumer), startTime, error);
            }
        }
    }
}