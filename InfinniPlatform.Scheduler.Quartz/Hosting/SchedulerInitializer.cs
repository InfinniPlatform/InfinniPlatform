﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.Hosting;
using InfinniPlatform.Scheduler.Properties;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Scheduler.Hosting
{
    /// <summary>
    /// Осуществляет запуск и остановку планирования заданий соответственно при запуске и остановке приложения.
    /// </summary>
    /// <remarks>
    /// Планировщик заданий должен начать свою работу после того, как все подсистемы будут запущены. По этой причине
    /// запуск планировщика осуществляется в реализации <see cref="IAppStartedHandler" />. Чтобы вызов данного метода не
    /// увеличивал время запуска приложения, запуск планировщика осуществляется асинхронно, с помощью метода
    /// <see cref="Task.Run(Func{Task})" />. Из этих же соображений остановка планировщика осуществляется
    /// в реализации <see cref="IAppStoppedHandler" /> также асинхронно.
    /// </remarks>
    internal class SchedulerInitializer : IAppStartedHandler, IAppStoppedHandler
    {
        public SchedulerInitializer(IJobSchedulerDispatcher jobSchedulerDispatcher,
                                    IEnumerable<IJobInfoSource> jobInfoSources,
                                    IJobInfoFactory jobInfoFactory,
                                    ILogger<SchedulerInitializer> logger)
        {
            _jobSchedulerDispatcher = jobSchedulerDispatcher;
            _jobInfoSources = jobInfoSources;
            _jobInfoFactory = jobInfoFactory;
            _logger = logger;
        }


        private readonly IJobSchedulerDispatcher _jobSchedulerDispatcher;
        private readonly IEnumerable<IJobInfoSource> _jobInfoSources;
        private readonly IJobInfoFactory _jobInfoFactory;
        private readonly ILogger _logger;


        /// <summary>
        /// Вызывается после запуска основных компонентов приложения.
        /// </summary>
        void IAppStartedHandler.Handle()
        {
            Task.Run(TryToStartJobScheduler);
        }

        /// <summary>
        /// Вызывается перед остановкой основных компонентов приложения.
        /// </summary>
        void IAppStoppedHandler.Handle()
        {
            Task.Run(TryToStopJobScheduler);
        }


        /// <summary>
        /// Производит попытку запуска планировщика заданий.
        /// </summary>
        private async Task TryToStartJobScheduler()
        {
            _logger.LogInformation(Resources.StartingJobScheduler);

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

                _logger.LogInformation(Resources.StartingJobSchedulerSuccessfullyCompleted);
            }
            catch (Exception exception)
            {
                _logger.LogError(Resources.StartingJobSchedulerCompletedWithException, exception);
            }
        }

        /// <summary>
        /// Производит попутку добавления заданий указанного источника.
        /// </summary>
        private async Task TryToScheduleJobSource(IJobInfoSource jobInfoSource)
        {
            Func<Dictionary<string, object>> logContext = () => new Dictionary<string, object> { { "jobInfoSource", jobInfoSource.GetType().AssemblyQualifiedName } };

            _logger.LogDebug(Resources.AddingJobSourceToSchedule, logContext);

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

                _logger.LogDebug(Resources.AddingJobSourceToScheduleSuccessfullyCompleted, logContext);
            }
            catch (Exception exception)
            {
                _logger.LogError(Resources.AddingJobSourceToScheduleCompletedWithException, exception, logContext);
            }
        }

        /// <summary>
        /// Производит попутку добавления указанного задания.
        /// </summary>
        private async Task TryToScheduleJob(IJobInfo jobInfo)
        {
            Func<Dictionary<string, object>> logContext = () => new Dictionary<string, object> { { "jobId", jobInfo.Id } };

            _logger.LogDebug(Resources.AddingJobToSchedule, logContext);

            try
            {
                // Добавление или обновление задания
                await _jobSchedulerDispatcher.AddOrUpdateJob(jobInfo);

                _logger.LogDebug(Resources.AddingJobToScheduleSuccessfullyCompleted, logContext);
            }
            catch (Exception exception)
            {
                _logger.LogError(Resources.AddingJobToScheduleCompletedWithException, exception, logContext);
            }
        }

        /// <summary>
        /// Производит попытку остановки планировщика заданий.
        /// </summary>
        private async Task TryToStopJobScheduler()
        {
            _logger.LogInformation(Resources.StoppingJobScheduler);

            try
            {
                // Остановка планирования заданий
                await _jobSchedulerDispatcher.Stop();

                _logger.LogInformation(Resources.StoppingJobSchedulerSuccessfullyCompleted);
            }
            catch (Exception exception)
            {
                _logger.LogError(Resources.StoppingJobSchedulerCompletedWithException, exception);
            }
        }
    }
}