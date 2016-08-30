using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Scheduler.Contract;

using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Quartz.Spi;

namespace InfinniPlatform.Scheduler.Quartz
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
            _jobKeys = new ConcurrentDictionary<string, JobKey>(StringComparer.Ordinal);
        }


        private readonly IJobFactory _jobFactory;
        private readonly ILogProvider _logProvider;
        private readonly Lazy<Task<IScheduler>> _scheduler;
        private readonly ConcurrentDictionary<string, JobKey> _jobKeys;


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


        public bool IsJobExists(string jobId)
        {
            if (string.IsNullOrEmpty(jobId))
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            return _jobKeys.ContainsKey(jobId);
        }


        public async Task AddOrUpdateJob(IJobInfo jobInfo)
        {
            if (jobInfo == null)
            {
                throw new ArgumentNullException(nameof(jobInfo));
            }

            var jobKey = new JobKey(jobInfo.Name, jobInfo.Group);

            // Прекращение планирования предыдущего экземпляра задания
            await SchedulerAction(s => s.DeleteJob(jobKey));

            // Невозможно добавить приостановленное задание
            if (jobInfo.State == JobState.Paused)
            {
                return;
            }

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

            // Добавление ключа задания в список
            _jobKeys[jobInfo.Id] = jobKey;
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
            var jobKey = FindJobKey(jobId);

            if (jobKey != null)
            {
                // Прекращение планирования существующего экземпляра задания
                await SchedulerAction(s => s.DeleteJob(jobKey));

                // Удаление ключа задания из списка
                JobKey deletedJobKey;
                _jobKeys.TryRemove(jobId, out deletedJobKey);
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
            var allJobIds = _jobKeys.Keys.ToList();

            return DeleteJobs(allJobIds);
        }


        public async Task PauseJob(string jobId)
        {
            var jobKey = FindJobKey(jobId);

            if (jobKey != null)
            {
                // Приостановка планирования существующего экземпляра задания
                await SchedulerAction(s => s.PauseJob(jobKey));
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
            var allJobIds = _jobKeys.Keys.ToList();

            return PauseJobs(allJobIds);
        }


        public async Task ResumeJob(string jobId)
        {
            var jobKey = FindJobKey(jobId);

            if (jobKey != null)
            {
                // Возобновление планирования существующего экземпляра задания
                await SchedulerAction(s => s.ResumeJob(jobKey));
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
            var allJobIds = _jobKeys.Keys.ToList();

            return ResumeJobs(allJobIds);
        }


        public async Task TriggerJob(string jobId, object data = null)
        {
            var jobKey = FindJobKey(jobId);

            if (jobKey != null)
            {
                // Досрочное выполнение существующего экземпляра задания

                if (data == null)
                {
                    await SchedulerAction(s => s.TriggerJob(jobKey));
                }
                else
                {
                    var jobData = new JobDataMap { { QuartzJob.TriggerDataKey, data } };

                    await SchedulerAction(s => s.TriggerJob(jobKey, jobData));
                }
            }
        }

        public async Task TriggerJobs(IEnumerable<string> jobIds, object data = null)
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

        public Task TriggerAllJob(object data = null)
        {
            var allJobIds = _jobKeys.Keys.ToList();

            return TriggerJobs(allJobIds, data);
        }


        public Task Start()
        {
            return SchedulerAction(s => s.Start());
        }

        public Task Stop()
        {
            return SchedulerAction(s => s.Shutdown());
        }


        private JobKey FindJobKey(string jobId)
        {
            if (string.IsNullOrEmpty(jobId))
            {
                throw new ArgumentNullException(nameof(jobId));
            }

            JobKey jobKey;

            _jobKeys.TryGetValue(jobId, out jobKey);

            return jobKey;
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
    }
}