using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues.Producers;

using Quartz;

namespace InfinniPlatform.Scheduler.Quartz
{
    /// <summary>
    /// Обработчик заданий Quartz.
    /// </summary>
    internal class QuartzJob : IJob
    {
        /// <summary>
        /// Ключ для доступа к информации о задании.
        /// </summary>
        public const string JobInfoKey = "JobInfo";

        /// <summary>
        /// Ключ для доступа к данным выполнения задания при досрочном выполнении.
        /// </summary>
        public const string TriggerDataKey = "TriggerData";


        public QuartzJob(IJobInstanceFactory jobInstanceFactory,
                         IJobInstanceManager jobInstanceManager,
                         ITaskProducer taskProducer)
        {
            _jobInstanceFactory = jobInstanceFactory;
            _jobInstanceManager = jobInstanceManager;
            _taskProducer = taskProducer;
        }


        private readonly IJobInstanceFactory _jobInstanceFactory;
        private readonly IJobInstanceManager _jobInstanceManager;
        private readonly ITaskProducer _taskProducer;


        public async Task Execute(IJobExecutionContext context)
        {
            // Определение информации о здании

            var jobInfo = context.MergedJobDataMap.Get(JobInfoKey) as JobInfo;

            if (jobInfo == null)
            {
                return;
            }

            // Определение времени срабатывания задания

            var fireTimeUtc = context.FireTimeUtc;
            var scheduledFireTimeUtc = context.ScheduledFireTimeUtc;

            if (fireTimeUtc == null || scheduledFireTimeUtc == null)
            {
                return;
            }

            // Создание уникального идентификатора экземпляра задания

            var jobInstance = _jobInstanceFactory.CreateJobInstance(jobInfo.Id, scheduledFireTimeUtc.Value);

            // Проверка факта обработки задания кем-то ранее

            if (await _jobInstanceManager.IsHandled(jobInstance))
            {
                return;
            }

            // Создание контекста выполнения задания

            var triggerData = context.MergedJobDataMap.Get(TriggerDataKey) as DynamicWrapper;

            var jobHandlerContext = new JobHandlerContext
            {
                InstanceId = jobInstance,
                FireTimeUtc = fireTimeUtc.Value,
                ScheduledFireTimeUtc = scheduledFireTimeUtc.Value,
                PreviousFireTimeUtc = context.PreviousFireTimeUtc,
                NextFireTimeUtc = context.NextFireTimeUtc,
                Data = triggerData ?? jobInfo.Data
            };

            var handleJobMessage = new JobHandlerEvent
            {
                JobInfo = jobInfo,
                Context = jobHandlerContext
            };

            // Постановка задания в очередь на выполнение
            await _taskProducer.PublishAsync(handleJobMessage);
        }
    }
}