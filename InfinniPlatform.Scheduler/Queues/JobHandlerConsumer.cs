using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Scheduler.Properties;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

namespace InfinniPlatform.Scheduler.Queues
{
    /// <summary>
    /// Обработчик события <see cref="JobHandlerEvent"/>.
    /// </summary>
    internal class JobHandlerConsumer : TaskConsumerBase<JobHandlerEvent>
    {
        public JobHandlerConsumer(IJobHandlerTypeSerializer jobHandlerTypeSerializer,
                                  IPerformanceLog performanceLog,
                                  ILog log)
        {
            _jobHandlerTypeSerializer = jobHandlerTypeSerializer;
            _performanceLog = performanceLog;
            _log = log;
        }


        private readonly IJobHandlerTypeSerializer _jobHandlerTypeSerializer;
        private readonly IPerformanceLog _performanceLog;
        private readonly ILog _log;


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

                // Имя типа обработчика задания
                var jobHandlerType = jobInfo.HandlerType;

                // Создание экземпляра обработчика задания
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

                _log.Error(Resources.HandlingOfJobCompletedWithException, exception, logContext);
            }
            finally
            {
                _performanceLog.Log(nameof(JobHandlerConsumer), startTime, error);
            }
        }
    }
}