﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;

using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Quartz.Spi;

namespace InfinniPlatform.Scheduler.Dispatcher
{
    /// <summary>
    /// Диспетчер планировщика заданий <see cref="IJobScheduler"/> на основе Quartz.
    /// </summary>
    internal class QuartzJobSchedulerDispatcher : IJobSchedulerDispatcher
    {
        public QuartzJobSchedulerDispatcher(IJobFactory jobFactory, ILogProvider logProvider)
        {
            _jobFactory = jobFactory;
            _logProvider = logProvider;
            _scheduler = new Lazy<Task<IScheduler>>(() => Task.Run(CreateQuartzScheduler));
            _jobs = new ConcurrentDictionary<string, JobStatus>(StringComparer.Ordinal);
            _monitor = new AsyncMonitor();
        }


        private readonly IJobFactory _jobFactory;
        private readonly ILogProvider _logProvider;
        private readonly Lazy<Task<IScheduler>> _scheduler;
        private readonly ConcurrentDictionary<string, JobStatus> _jobs;
        private readonly AsyncMonitor _monitor;


        private async Task<IScheduler> CreateQuartzScheduler()
        {
            // Установка провайдера логирования событий Quartz
            LogProvider.SetCurrentLogProvider(_logProvider);

            // Определение настроек планировщика заданий Quartz
            var quartzSettings = new NameValueCollection();

            // Создание фабрики планировщиков заданий Quartz
            var schedulerFactory = new StdSchedulerFactory(quartzSettings);

            // Создание планировщика заданий Quartz
            var quartzScheduler = await schedulerFactory.GetScheduler();

            // Установка фабрики обработчиков заданий Quartz
            quartzScheduler.JobFactory = _jobFactory;

            return quartzScheduler;
        }


        public Task<bool> IsStarted()
        {
            return SchedulerAction(s => Task.FromResult(s.IsStarted));
        }

        public Task Start()
        {
            return SchedulerAction(s => s.Start());
        }

        public Task Stop()
        {
            return SchedulerAction(s => s.Shutdown());
        }


        public Task<TResult> GetStatus<TResult>(Func<IEnumerable<IJobStatus>, TResult> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            var result = selector(_jobs.Values);

            return Task.FromResult(result);
        }


        public async Task AddOrUpdateJob(IJobInfo jobInfo)
        {
            if (jobInfo == null)
            {
                throw new ArgumentNullException(nameof(jobInfo));
            }

            // Информация о состоянии задания
            var jobKey = new JobKey(jobInfo.Name, jobInfo.Group);
            var jobStatus = new JobStatus(jobKey, jobInfo, jobInfo.State);

            // Получение актуальной информации о состоянии задания
            jobStatus = _jobs.GetOrAdd(jobInfo.Id, jobStatus);

            // Блокировка экземпляра задания на время его изменения
            using (await _monitor.LockAsync(jobStatus))
            {
                // Обновление состояния задания
                jobStatus.Info = jobInfo;
                jobStatus.State = jobInfo.State;

                // Прекращение планирования предыдущего экземпляра задания
                await SchedulerAction(s => s.DeleteJob(jobKey));

                // Если задание запланировано для выполнения
                if (jobInfo.State == JobState.Planned)
                {
                    // Начало планирования нового экземпляра задания
                    await ScheduleJob(jobKey, jobInfo);
                }
            }
        }

        public async Task AddOrUpdateJobs(IEnumerable<IJobInfo> jobInfos)
        {
            if (jobInfos == null)
            {
                throw new ArgumentNullException(nameof(jobInfos));
            }

            foreach (var jobInfo in jobInfos)
            {
                await AddOrUpdateJob(jobInfo);
            }
        }


        public async Task DeleteJob(string jobId)
        {
            var jobItem = GetJobItem(jobId);

            if (jobItem != null)
            {
                JobStatus deletedJob;

                // Удаление ключа задания из списка
                _jobs.TryRemove(jobId, out deletedJob);

                // Блокировка экземпляра задания на время его изменения
                using (await _monitor.LockAsync(jobItem))
                {
                    // Прекращение планирования существующего экземпляра задания
                    await SchedulerAction(s => s.DeleteJob(jobItem.Key));
                }
            }
        }

        public async Task DeleteJobs(IEnumerable<string> jobIds)
        {
            if (jobIds == null)
            {
                throw new ArgumentNullException(nameof(jobIds));
            }

            foreach (var jobId in jobIds)
            {
                await DeleteJob(jobId);
            }
        }

        public Task DeleteAllJobs()
        {
            var allJobIds = _jobs.Keys.ToList();

            return DeleteJobs(allJobIds);
        }


        public async Task PauseJob(string jobId)
        {
            var jobItem = GetJobItem(jobId);

            if (jobItem != null)
            {
                // Блокировка экземпляра задания на время его изменения
                using (await _monitor.LockAsync(jobItem))
                {
                    // Обновление состояние задания
                    jobItem.State = JobState.Paused;

                    // Приостановка планирования существующего экземпляра задания
                    await SchedulerAction(s => s.PauseJob(jobItem.Key));
                }
            }
        }

        public async Task PauseJobs(IEnumerable<string> jobIds)
        {
            if (jobIds == null)
            {
                throw new ArgumentNullException(nameof(jobIds));
            }

            foreach (var jobId in jobIds)
            {
                await PauseJob(jobId);
            }
        }

        public Task PauseAllJobs()
        {
            var allJobIds = _jobs.Keys.ToList();

            return PauseJobs(allJobIds);
        }


        public async Task ResumeJob(string jobId)
        {
            var jobItem = GetJobItem(jobId);

            if (jobItem != null)
            {
                // Блокировка экземпляра задания на время его изменения
                using (await _monitor.LockAsync(jobItem))
                {
                    // Обновление состояние задания
                    jobItem.State = JobState.Paused;

                    if (await SchedulerAction(s => s.CheckExists(jobItem.Key)))
                    {
                        // Возобновление планирования существующего экземпляра задания
                        await SchedulerAction(s => s.ResumeJob(jobItem.Key));
                    }
                    else
                    {
                        // Начало планирования нового экземпляра задания
                        await ScheduleJob(jobItem.Key, jobItem.Info);
                    }
                }
            }
        }

        public async Task ResumeJobs(IEnumerable<string> jobIds)
        {
            if (jobIds == null)
            {
                throw new ArgumentNullException(nameof(jobIds));
            }

            foreach (var jobId in jobIds)
            {
                await ResumeJob(jobId);
            }
        }

        public Task ResumeAllJobs()
        {
            var allJobIds = _jobs.Keys.ToList();

            return ResumeJobs(allJobIds);
        }


        public async Task TriggerJob(string jobId, DynamicDocument data = null)
        {
            var jobItem = GetJobItem(jobId);

            if (jobItem != null)
            {
                // Досрочное выполнение существующего экземпляра задания

                if (data == null)
                {
                    await SchedulerAction(s => s.TriggerJob(jobItem.Key));
                }
                else
                {
                    var jobData = new JobDataMap { { QuartzJob.TriggerDataKey, data } };

                    await SchedulerAction(s => s.TriggerJob(jobItem.Key, jobData));
                }
            }
        }

        public async Task TriggerJobs(IEnumerable<string> jobIds, DynamicDocument data = null)
        {
            if (jobIds == null)
            {
                throw new ArgumentNullException(nameof(jobIds));
            }

            foreach (var jobId in jobIds)
            {
                await TriggerJob(jobId, data);
            }
        }

        public Task TriggerAllJob(DynamicDocument data = null)
        {
            var allJobIds = _jobs.Keys.ToList();

            return TriggerJobs(allJobIds, data);
        }


        private JobStatus GetJobItem(string jobId)
        {
            if (string.IsNullOrEmpty(jobId))
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            JobStatus jobStatus;

            _jobs.TryGetValue(jobId, out jobStatus);

            return jobStatus;
        }

        private async Task ScheduleJob(JobKey jobKey, IJobInfo jobInfo)
        {
            var jobData = new JobDataMap { { QuartzJob.JobInfoKey, jobInfo } };

            // Создание экземпляра задания

            var jobDetail = JobBuilder
                .Create<QuartzJob>()
                // Не освобождать экземпляр задания при его удалении
                .StoreDurably()
                // Исполнять задание повторно при сбоях планировщика
                .RequestRecovery()
                // Установка сведений о экземпляре задания
                .WithIdentity(jobKey)
                .UsingJobData(jobData)
                .WithDescription(jobInfo.Description)
                // Формирование экземпляра задания
                .Build();

            // Создание экземпляра триггера

            var jobTriggerBuilder = TriggerBuilder
                .Create()
                .WithIdentity(jobInfo.Name, jobInfo.Group)
                .UsingJobData(jobData)
                .WithDescription(jobInfo.Description);

            if (!string.IsNullOrWhiteSpace(jobInfo.CronExpression))
            {
                if (jobInfo.MisfirePolicy == JobMisfirePolicy.FireAndProceed)
                {
                    jobTriggerBuilder.WithCronSchedule(jobInfo.CronExpression, c => c.WithMisfireHandlingInstructionFireAndProceed());
                }
                else
                {
                    jobTriggerBuilder.WithCronSchedule(jobInfo.CronExpression, c => c.WithMisfireHandlingInstructionDoNothing());
                }
            }

            if (jobInfo.StartTimeUtc != null)
            {
                jobTriggerBuilder.StartAt(jobInfo.StartTimeUtc.Value);
            }

            if (jobInfo.EndTimeUtc != null)
            {
                jobTriggerBuilder.EndAt(jobInfo.EndTimeUtc.Value);
            }

            var jobTrigger = jobTriggerBuilder.Build();

            // Начало планирования нового экземпляра задания
            await SchedulerAction(s => s.ScheduleJob(jobDetail, jobTrigger));
        }


        private async Task SchedulerAction(Func<IScheduler, Task> action)
        {
            var scheduler = await _scheduler.Value;
            await action(scheduler);
        }

        private async Task<TResult> SchedulerAction<TResult>(Func<IScheduler, Task<TResult>> action)
        {
            var scheduler = await _scheduler.Value;
            return await action(scheduler);
        }


        private class JobStatus : IJobStatus
        {
            public JobStatus(JobKey key, IJobInfo info, JobState state)
            {
                Key = key;
                Info = info;
                State = state;
            }

            public readonly JobKey Key;

            public IJobInfo Info { get; set; }

            public JobState State { get; set; }
        }
    }
}