using System.Threading.Tasks;

using InfinniPlatform.Dynamic;

using Quartz;

namespace InfinniPlatform.Scheduler.Dispatcher
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
                         IJobSchedulerRepository jobSchedulerRepository,
                         IJobSchedulerStateObserver jobSchedulerStateObserver)
        {
            _jobInstanceFactory = jobInstanceFactory;
            _jobSchedulerRepository = jobSchedulerRepository;
            _jobSchedulerStateObserver = jobSchedulerStateObserver;
        }


        private readonly IJobInstanceFactory _jobInstanceFactory;
        private readonly IJobSchedulerRepository _jobSchedulerRepository;
        private readonly IJobSchedulerStateObserver _jobSchedulerStateObserver;


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

            if (scheduledFireTimeUtc == null)
            {
                return;
            }

            // Создание уникального идентификатора экземпляра задания

            var jobInstance = _jobInstanceFactory.CreateJobInstance(jobInfo.Id, scheduledFireTimeUtc.Value);

            // Проверка факта обработки задания кем-то ранее

            if (await _jobSchedulerRepository.IsHandledJob(jobInstance))
            {
                return;
            }

            // Создание контекста выполнения задания

            var triggerData = context.MergedJobDataMap.Get(TriggerDataKey) as DynamicDocument;

            var jobHandlerContext = new JobHandlerContext
                                    {
                                        InstanceId = jobInstance,
                                        FireTimeUtc = fireTimeUtc,
                                        ScheduledFireTimeUtc = scheduledFireTimeUtc.Value,
                                        PreviousFireTimeUtc = context.PreviousFireTimeUtc,
                                        NextFireTimeUtc = context.NextFireTimeUtc,
                                        Data = triggerData ?? jobInfo.Data
                                    };

            // Постановка задания в очередь на выполнение
            await _jobSchedulerStateObserver.OnExecuteJob(jobInfo, jobHandlerContext);
        }
    }
}