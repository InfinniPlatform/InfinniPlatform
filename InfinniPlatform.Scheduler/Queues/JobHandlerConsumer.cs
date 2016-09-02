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
    /// Обработчик событий о необходимости обработать задание.
    /// </summary>
    internal class JobHandlerConsumer : TaskConsumerBase<JobHandlerEvent>
    {
        public JobHandlerConsumer(IJobHandlerTypeSerializer handlerTypeSerializer,
                                 ILog log)
        {
            _handlerTypeSerializer = handlerTypeSerializer;
            _log = log;
        }


        private readonly IJobHandlerTypeSerializer _handlerTypeSerializer;
        private readonly ILog _log;


        protected override async Task Consume(Message<JobHandlerEvent> message)
        {
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
                var handler = _handlerTypeSerializer.Deserialize(jobHandlerType);

                if (handler != null)
                {
                    // Обработка задания
                    await handler.Handle(jobInfo, context);
                }
            }
            catch (Exception exception)
            {
                Func<Dictionary<string, object>> logContext = () => new Dictionary<string, object>
                                                                    {
                                                                        { "jobId", jobInfo?.Id },
                                                                        { "jobInstanceId", context?.InstanceId },
                                                                        { "jobHandlerType", jobInfo?.HandlerType }
                                                                    };

                _log.Error(Resources.HandlingOfJobCompletedWithException, exception, logContext);
            }
        }
    }
}