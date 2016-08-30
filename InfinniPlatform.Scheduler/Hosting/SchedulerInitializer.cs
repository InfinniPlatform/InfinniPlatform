using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Scheduler.Properties;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.Scheduler.Hosting
{
    /// <summary>
    /// Осуществляет запуск и остановку планирования заданий соответственно при запуске и остановке приложения.
    /// </summary>
    /// <remarks>
    /// Планировщик заданий должен начать свою работу после того, как все подсистемы будут запущены. По этой причине
    /// запуск планировщика осуществляется в методе <see cref="OnAfterStart" />. Чтобы вызов данного метода не
    /// увеличивал время запуска приложения, запуск планировщика осуществляется асинхронно, с помощью метода
    /// <see cref="Task.Run(Func{Task})" />. Из этих же соображений остановка планировщика осуществляется
    /// в методе <see cref="OnBeforeStop" /> также асинхронно.
    /// </remarks>
    internal class SchedulerInitializer : AppEventHandler
    {
        public SchedulerInitializer(IJobSchedulerDispatcher jobSchedulerDispatcher,
                                    IEnumerable<IJobInfoSource> jobInfoSources,
                                    IJobInfoFactory jobInfoFactory,
                                    ILog log)
        {
            _jobSchedulerDispatcher = jobSchedulerDispatcher;
            _jobInfoSources = jobInfoSources;
            _jobInfoFactory = jobInfoFactory;
            _log = log;
        }


        private readonly IJobSchedulerDispatcher _jobSchedulerDispatcher;
        private readonly IEnumerable<IJobInfoSource> _jobInfoSources;
        private readonly IJobInfoFactory _jobInfoFactory;
        private readonly ILog _log;


        /// <summary>
        /// Вызывается после запуска основных компонентов приложения.
        /// </summary>
        public override void OnAfterStart()
        {
            Task.Run(TryToStartJobScheduler);
        }

        /// <summary>
        /// Вызывается перед остановкой основных компонентов приложения.
        /// </summary>
        public override void OnBeforeStop()
        {
            Task.Run(TryToStopJobScheduler);
        }


        /// <summary>
        /// Производит попытку запуска планировщика заданий.
        /// </summary>
        private async Task TryToStartJobScheduler()
        {
            _log.Info(Resources.StartingJobScheduler);

            try
            {
                if (_jobInfoSources != null)
                {
                    foreach (var jobInfoSource in _jobInfoSources)
                    {
                        // Добавление заданий источника
                        await TryToScheduleJobSource(jobInfoSource);
                    }
                }

                // Запуск планирования заданий
                await _jobSchedulerDispatcher.Start();

                _log.Info(Resources.StartingJobSchedulerSuccessfullyCompleted);
            }
            catch (Exception exception)
            {
                _log.Error(Resources.StartingJobSchedulerCompletedWithException, exception);
            }
        }

        /// <summary>
        /// Производит попутку добавления заданий указанного источника.
        /// </summary>
        private async Task TryToScheduleJobSource(IJobInfoSource jobInfoSource)
        {
            Func<Dictionary<string, object>> logContext = () => new Dictionary<string, object> { { "jobInfoSource", jobInfoSource.GetType().AssemblyQualifiedName } };

            _log.Debug(Resources.AddingJobSourceToSchedule, logContext);

            try
            {
                // Получение списка заданий источника
                var jobInfos = await jobInfoSource.GetJobs(_jobInfoFactory);

                if (jobInfos != null)
                {
                    foreach (var jobInfo in jobInfos)
                    {
                        // Добавление задания
                        await TryToScheduleJob(jobInfo);
                    }
                }

                _log.Debug(Resources.AddingJobSourceToScheduleSuccessfullyCompleted, logContext);
            }
            catch (Exception exception)
            {
                _log.Error(Resources.AddingJobSourceToScheduleCompletedWithException, exception, logContext);
            }
        }

        /// <summary>
        /// Производит попутку добавления указанного задания.
        /// </summary>
        private async Task TryToScheduleJob(IJobInfo jobInfo)
        {
            Func<Dictionary<string, object>> logContext = () => new Dictionary<string, object> { { "jobId", jobInfo.Id } };

            _log.Debug(Resources.AddingJobToSchedule, logContext);

            try
            {
                // Добавление или обновление задания
                await _jobSchedulerDispatcher.AddOrUpdateJob(jobInfo);

                _log.Debug(Resources.AddingJobToScheduleSuccessfullyCompleted, logContext);
            }
            catch (Exception exception)
            {
                _log.Error(Resources.AddingJobToScheduleCompletedWithException, exception, logContext);
            }
        }

        /// <summary>
        /// Производит попытку остановки планировщика заданий.
        /// </summary>
        private async Task TryToStopJobScheduler()
        {
            _log.Info(Resources.StoppingJobScheduler);

            try
            {
                // Остановка планирования заданий
                await _jobSchedulerDispatcher.Stop();

                _log.Info(Resources.StoppingJobSchedulerSuccessfullyCompleted);
            }
            catch (Exception exception)
            {
                _log.Error(Resources.StoppingJobSchedulerCompletedWithException, exception);
            }
        }
    }
}