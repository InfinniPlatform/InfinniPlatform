using System;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;
using InfinniPlatform.Scheduler.Common;

using NUnit.Framework;

using Quartz;
using Quartz.Logging;
using Quartz.Spi;

namespace InfinniPlatform.Scheduler.Dispatcher
{
    [TestFixture(Category = TestCategories.UnitTest)]
    public class QuartzJobSchedulerDispatcherTest
    {
        private const int DefaultWaitTimeout = 1000;
        private const string JobInfoKey = "JobInfo";
        private const string TriggerDataKey = "TriggerData";
        private const string EverySecondCronExpression = "* * * * * ?";


        [Test(Description = "Добавление нового задания до запуска диспетчера")]
        public async Task ShouldAddNewJobBeforeStart()
        {
            // Given

            var jobInfo = CreateJobInfo();
            var expectedContext = new TaskCompletionSource<IJobExecutionContext>();
            Action<IJobExecutionContext> jobAction = c => { expectedContext.SetResult(c); };

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление задания
            await dispatcher.AddOrUpdateJob(jobInfo);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание вызова обработчика заданий
                await expectedContext.Task;
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsNotNull(expectedContext.Task.Result);
            Assert.AreEqual(jobInfo, expectedContext.Task.Result.MergedJobDataMap.Get(JobInfoKey));
            Assert.AreEqual(jobInfo.Group, expectedContext.Task.Result.JobDetail.Key.Group);
            Assert.AreEqual(jobInfo.Name, expectedContext.Task.Result.JobDetail.Key.Name);
            Assert.AreEqual(jobInfo.Description, expectedContext.Task.Result.JobDetail.Description);
        }

        [Test(Description = "Добавление нового задания после запуска диспетчера")]
        public async Task ShouldAddNewJobAfterStart()
        {
            // Given

            var jobInfo = CreateJobInfo();
            var expectedContext = new TaskCompletionSource<IJobExecutionContext>();
            Action<IJobExecutionContext> jobAction = c => { expectedContext.SetResult(c); };

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Добавление задания
                await dispatcher.AddOrUpdateJob(jobInfo);

                // Ожидание вызова обработчика заданий
                await expectedContext.Task;
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsNotNull(expectedContext.Task.Result);
            Assert.AreEqual(jobInfo, expectedContext.Task.Result.MergedJobDataMap.Get(JobInfoKey));
            Assert.AreEqual(jobInfo.Group, expectedContext.Task.Result.JobDetail.Key.Group);
            Assert.AreEqual(jobInfo.Name, expectedContext.Task.Result.JobDetail.Key.Name);
            Assert.AreEqual(jobInfo.Description, expectedContext.Task.Result.JobDetail.Description);
        }

        [Test(Description = "Обновление существующего задания до запуска диспетчера")]
        public async Task ShouldUpdateExistsJobBeforeStart()
        {
            // Given

            var jobInfo1 = CreateJobInfo(description: "Description1");
            var jobInfo2 = CreateJobInfo(description: "Description2");

            var beforeUpdate = new TaskCompletionSource<IJobExecutionContext>();
            var afterUpdate = new TaskCompletionSource<IJobExecutionContext>();

            Action<IJobExecutionContext> jobAction = c =>
                                                     {
                                                         if (c.JobDetail.Description == jobInfo1.Description)
                                                         {
                                                             beforeUpdate.SetResult(c);
                                                         }
                                                         else if (c.JobDetail.Description == jobInfo2.Description)
                                                         {
                                                             afterUpdate.SetResult(c);
                                                         }
                                                     };

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление задания
            await dispatcher.AddOrUpdateJob(jobInfo1);

            // Обновление задания
            await dispatcher.AddOrUpdateJob(jobInfo2);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание вызова первой версии обработчика заданий (не должен сработать)
                beforeUpdate.Task.Wait(DefaultWaitTimeout);

                // Ожидание вызова второй версии обработчика заданий (должен сработать)
                await afterUpdate.Task;
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsFalse(beforeUpdate.Task.IsCompleted);
            Assert.IsTrue(afterUpdate.Task.IsCompleted);
        }

        [Test(Description = "Обновление существующего задания после запуска диспетчера")]
        public async Task ShouldUpdateExistsJobAfterStart()
        {
            // Given

            var jobInfo1 = CreateJobInfo(description: "Description1");
            var jobInfo2 = CreateJobInfo(description: "Description2");

            var beforeUpdate = new TaskCompletionSource<IJobExecutionContext>();
            var afterUpdate = new TaskCompletionSource<IJobExecutionContext>();

            Action<IJobExecutionContext> jobAction = c =>
                                                     {
                                                         if (c.JobDetail.Description == jobInfo1.Description)
                                                         {
                                                             beforeUpdate.SetResult(c);
                                                         }
                                                         else if (c.JobDetail.Description == jobInfo2.Description)
                                                         {
                                                             afterUpdate.SetResult(c);
                                                         }
                                                     };

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление задания
            await dispatcher.AddOrUpdateJob(jobInfo1);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание вызова первой версии обработчика заданий (должен сработать)
                await beforeUpdate.Task;

                // Обновление задания
                await dispatcher.AddOrUpdateJob(jobInfo2);

                // Ожидание вызова второй версии обработчика заданий (должен сработать)
                await afterUpdate.Task;
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsTrue(beforeUpdate.Task.IsCompleted);
            Assert.IsTrue(afterUpdate.Task.IsCompleted);
        }

        [Test(Description = "Добавление приостановленного задания до запуска диспетчера")]
        public async Task ShouldAddPausedJobBeforeStart()
        {
            // Given

            var jobInfo = CreateJobInfo(state: JobState.Paused);
            var expectedContext = new TaskCompletionSource<IJobExecutionContext>();
            Action<IJobExecutionContext> jobAction = c => { expectedContext.SetResult(c); };

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление приостановленного задания
            await dispatcher.AddOrUpdateJob(jobInfo);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание вызова обработчика приостановленного задания (не должен сработать)
                expectedContext.Task.Wait(DefaultWaitTimeout);
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsFalse(expectedContext.Task.IsCompleted);
        }

        [Test(Description = "Обновление приостановленного задания запланированным после запуска диспетчера")]
        public async Task ShouldUpdatePausedJobAfterStart()
        {
            // Given

            var jobInfo1 = CreateJobInfo(state: JobState.Paused, description: "Description1");
            var jobInfo2 = CreateJobInfo(state: JobState.Planned, description: "Description2");

            var beforeUpdate = new TaskCompletionSource<IJobExecutionContext>();
            var afterUpdate = new TaskCompletionSource<IJobExecutionContext>();

            Action<IJobExecutionContext> jobAction = c =>
                                                     {
                                                         if (c.JobDetail.Description == jobInfo1.Description)
                                                         {
                                                             beforeUpdate.SetResult(c);
                                                         }
                                                         else if (c.JobDetail.Description == jobInfo2.Description)
                                                         {
                                                             afterUpdate.SetResult(c);
                                                         }
                                                     };

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление приостановленного задания
            await dispatcher.AddOrUpdateJob(jobInfo1);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание вызова обработчика приостановленного задания (не должен сработать)
                beforeUpdate.Task.Wait(DefaultWaitTimeout);

                // Обновление приостановленного задания запланированным
                await dispatcher.AddOrUpdateJob(jobInfo2);

                // Ожидание вызова обработчика запланированного задания (должен сработать)
                await afterUpdate.Task;
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsFalse(beforeUpdate.Task.IsCompleted);
            Assert.IsTrue(afterUpdate.Task.IsCompleted);
        }

        [Test(Description = "Удаление задания до запуска диспетчера")]
        public async Task ShouldDeleteJobBeforeStart()
        {
            // Given

            var jobInfo = CreateJobInfo();
            var expectedContext = new TaskCompletionSource<IJobExecutionContext>();
            Action<IJobExecutionContext> jobAction = c => { expectedContext.SetResult(c); };

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление задания
            await dispatcher.AddOrUpdateJob(jobInfo);

            // Удаление задания
            await dispatcher.DeleteJob(jobInfo.Id);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание вызова обработчика удаленного задания (не должен сработать)
                expectedContext.Task.Wait(DefaultWaitTimeout);
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsFalse(expectedContext.Task.IsCompleted);
        }

        [Test(Description = "Удаление задания после запуска диспетчера")]
        public async Task ShouldDeleteJobAfterStart()
        {
            // Given

            var jobInfo = CreateJobInfo(cronExpression: EverySecondCronExpression);
            var isDeleted = new ValueContainer<bool>();
            var beforeDelete = new TaskCompletionSource<IJobExecutionContext>();
            var afterDelete = new TaskCompletionSource<IJobExecutionContext>();

            Action<IJobExecutionContext> jobAction = c =>
                                                     {
                                                         if (!isDeleted.Value)
                                                         {
                                                             beforeDelete.SetResult(c);
                                                         }
                                                         else
                                                         {
                                                             afterDelete.SetResult(c);
                                                         }
                                                     };

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление задания
            await dispatcher.AddOrUpdateJob(jobInfo);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание вызова обработчика заданий до его удаления (должен сработать)
                await beforeDelete.Task;

                // Удаление задания
                await dispatcher.DeleteJob(jobInfo.Id);
                isDeleted.Value = true;

                // Ожидание вызова обработчика заданий после его удаления (не должен сработать)
                afterDelete.Task.Wait(DefaultWaitTimeout);
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsTrue(beforeDelete.Task.IsCompleted);
            Assert.IsFalse(afterDelete.Task.IsCompleted);
        }

        [Test(Description = "Приостановка выполнения задания до запуска диспетчера")]
        public async Task ShouldPauseJobBeforeStart()
        {
            // Given

            var jobInfo = CreateJobInfo();
            var expectedContext = new TaskCompletionSource<IJobExecutionContext>();
            Action<IJobExecutionContext> jobAction = c => { expectedContext.SetResult(c); };

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление задания
            await dispatcher.AddOrUpdateJob(jobInfo);

            // Приостановка задания
            await dispatcher.PauseJob(jobInfo.Id);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание вызова обработчика приостановленного задания (не должен сработать)
                expectedContext.Task.Wait(DefaultWaitTimeout);
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsFalse(expectedContext.Task.IsCompleted);
        }

        [Test(Description = "Приостановка выполнения задания после запуска диспетчера")]
        public async Task ShouldPauseJobAfterStart()
        {
            // Given

            var jobInfo = CreateJobInfo();
            var isPaised = new ValueContainer<bool>();
            var beforePause = new TaskCompletionSource<IJobExecutionContext>();
            var afterPause = new TaskCompletionSource<IJobExecutionContext>();

            Action<IJobExecutionContext> jobAction = c =>
                                                     {
                                                         if (!isPaised.Value)
                                                         {
                                                             beforePause.SetResult(c);
                                                         }
                                                         else
                                                         {
                                                             afterPause.SetResult(c);
                                                         }
                                                     };

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление задания
            await dispatcher.AddOrUpdateJob(jobInfo);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание вызова обработчика заданий до его приостановки (должен сработать)
                await beforePause.Task;

                // Приостановка задания
                await dispatcher.PauseJob(jobInfo.Id);
                isPaised.Value = true;

                // Ожидание вызова обработчика заданий после его приостановки (не должен сработать)
                afterPause.Task.Wait(DefaultWaitTimeout);
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsTrue(beforePause.Task.IsCompleted);
            Assert.IsFalse(afterPause.Task.IsCompleted);
        }

        [Test(Description = "Возобновление выполнения задания до запуска диспетчера")]
        public async Task ShouldResumeJobBeforeStart()
        {
            // Given

            var jobInfo = CreateJobInfo(state: JobState.Paused);
            var expectedContext = new TaskCompletionSource<IJobExecutionContext>();
            Action<IJobExecutionContext> jobAction = c => { expectedContext.SetResult(c); };

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление задания
            await dispatcher.AddOrUpdateJob(jobInfo);

            // Возобновление задания
            await dispatcher.ResumeJob(jobInfo.Id);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание вызова обработчика возобновленного задания (должен сработать)
                await expectedContext.Task;
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsTrue(expectedContext.Task.IsCompleted);
        }

        [Test(Description = "Возобновление выполнения задания после запуска диспетчера")]
        public async Task ShouldResumeJobAfterStart()
        {
            // Given

            var jobInfo = CreateJobInfo(state: JobState.Paused);
            var isResumed = new ValueContainer<bool>();
            var beforeResume = new TaskCompletionSource<IJobExecutionContext>();
            var afterResume = new TaskCompletionSource<IJobExecutionContext>();

            Action<IJobExecutionContext> jobAction = c =>
                                                     {
                                                         if (!isResumed.Value)
                                                         {
                                                             beforeResume.SetResult(c);
                                                         }
                                                         else
                                                         {
                                                             afterResume.SetResult(c);
                                                         }
                                                     };

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление задания
            await dispatcher.AddOrUpdateJob(jobInfo);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание вызова обработчика заданий до его возобновления (не должен сработать)
                beforeResume.Task.Wait(DefaultWaitTimeout);

                // Возобновление задания
                isResumed.Value = true;
                await dispatcher.ResumeJob(jobInfo.Id);

                // Ожидание вызова обработчика заданий после его возобновления (должен сработать)
                await afterResume.Task;
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsFalse(beforeResume.Task.IsCompleted);
            Assert.IsTrue(afterResume.Task.IsCompleted);
        }

        [Test(Description = "Досрочное выполнения задания до запуска диспетчера")]
        public async Task ShouldTriggerJobBeforeStart()
        {
            // Given

            var jobInfo = CreateJobInfo(startTimeUtc: DateTimeOffset.UtcNow.AddDays(1));
            var expectedContext = new TaskCompletionSource<IJobExecutionContext>();
            Action<IJobExecutionContext> jobAction = c => { expectedContext.SetResult(c); };

            var triggerData = new DynamicWrapper();

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление задания
            await dispatcher.AddOrUpdateJob(jobInfo);

            // Вызов досрочного выполнения задания
            await dispatcher.TriggerJob(jobInfo.Id, triggerData);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание досрочного вызова обработчика заданий (должен сработать)
                await expectedContext.Task;
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsNotNull(expectedContext.Task.Result);
            Assert.AreEqual(triggerData, expectedContext.Task.Result.MergedJobDataMap.Get(TriggerDataKey));
        }

        [Test(Description = "Досрочное выполнения задания после запуска диспетчера")]
        public async Task ShouldTriggerJobAfterStart()
        {
            // Given

            var jobInfo = CreateJobInfo(startTimeUtc: DateTimeOffset.UtcNow.AddDays(1));
            var isTriggered = new ValueContainer<bool>();
            var beforeTrigger = new TaskCompletionSource<IJobExecutionContext>();
            var afterTrigger = new TaskCompletionSource<IJobExecutionContext>();

            Action<IJobExecutionContext> jobAction = c =>
                                                     {
                                                         if (!isTriggered.Value)
                                                         {
                                                             beforeTrigger.SetResult(c);
                                                         }
                                                         else
                                                         {
                                                             afterTrigger.SetResult(c);
                                                         }
                                                     };

            var triggerData = new DynamicWrapper();

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление задания
            await dispatcher.AddOrUpdateJob(jobInfo);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание вызова обработчика заданий до вызова досрочного выполнения (не должен сработать)
                beforeTrigger.Task.Wait(DefaultWaitTimeout);

                // Вызов досрочного выполнения задания
                isTriggered.Value = true;
                await dispatcher.TriggerJob(jobInfo.Id, triggerData);

                // Ожидание вызова обработчика заданий после вызова досрочного выполнения (должен сработать)
                await afterTrigger.Task;
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsFalse(beforeTrigger.Task.IsCompleted);
            Assert.IsTrue(afterTrigger.Task.IsCompleted);
            Assert.IsNotNull(afterTrigger.Task.Result);
            Assert.AreEqual(triggerData, afterTrigger.Task.Result.MergedJobDataMap.Get(TriggerDataKey));
        }

        [Test(Description = "Применение политики обработки пропущенных заданий")]
        public async Task ShouldApplyMisfirePolicy()
        {
            // Given

            var startTimeInPast = DateTimeOffset.UtcNow.AddDays(-2);
            var endTimeInPast = DateTimeOffset.UtcNow.AddDays(-1);

            var jobInfo1 = CreateJobInfo("Job1",
                                         startTimeUtc: startTimeInPast,
                                         endTimeUtc: endTimeInPast,
                                         cronExpression: EverySecondCronExpression,
                                         misfirePolicy: JobMisfirePolicy.DoNothing);

            var jobInfo2 = CreateJobInfo("Job2",
                                         startTimeUtc: startTimeInPast,
                                         endTimeUtc: endTimeInPast,
                                         cronExpression: EverySecondCronExpression,
                                         misfirePolicy: JobMisfirePolicy.FireAndProceed);

            var jobContext1 = new TaskCompletionSource<IJobExecutionContext>();
            var jobContext2 = new TaskCompletionSource<IJobExecutionContext>();

            Action<IJobExecutionContext> jobAction = c =>
                                                     {
                                                         if (c.JobDetail.Key.Name == jobInfo1.Name)
                                                         {
                                                             jobContext1.SetResult(c);
                                                         }
                                                         else if (c.JobDetail.Key.Name == jobInfo2.Name)
                                                         {
                                                             jobContext2.SetResult(c);
                                                         }
                                                     };

            var dispatcher = CreateDispatcher(jobAction);

            // When

            // Добавление задания
            await dispatcher.AddOrUpdateJob(jobInfo1);
            await dispatcher.AddOrUpdateJob(jobInfo2);

            // Запуск диспетчера
            await dispatcher.Start();

            try
            {
                // Ожидание вызова обработчика заданий, которое игнорирует пропущенные задания (не должен сработать)
                jobContext1.Task.Wait(DefaultWaitTimeout);

                // Ожидание вызова обработчика заданий, которое не игнорирует пропущенные задания (должен сработать)
                await jobContext2.Task;
            }
            finally
            {
                // Остановка диспетчера
                await dispatcher.Stop();
            }

            // Then

            Assert.IsFalse(jobContext1.Task.IsCompleted);
            Assert.IsTrue(jobContext2.Task.IsCompleted);
        }

        private static IJobInfo CreateJobInfo(string name = null,
                                              JobState state = JobState.Planned,
                                              string description = null,
                                              string cronExpression = null,
                                              DateTimeOffset? startTimeUtc = null,
                                              DateTimeOffset? endTimeUtc = null,
                                              JobMisfirePolicy misfirePolicy = JobMisfirePolicy.DoNothing)
        {
            name = name ?? "Job1";
            description = description ?? "Description1";

            return new JobInfo
                   {
                       Id = "Group1." + name,
                       Name = name,
                       Group = "Group1",
                       State = state,
                       Description = description,
                       CronExpression = cronExpression,
                       StartTimeUtc = startTimeUtc,
                       EndTimeUtc = endTimeUtc,
                       MisfirePolicy = misfirePolicy
                   };
        }


        private static QuartzJobSchedulerDispatcher CreateDispatcher(Action<IJobExecutionContext> jobAction)
        {
            return new QuartzJobSchedulerDispatcher(new QuartzJobFactoryStub(jobAction), new QuartzLogProviderStub());
        }


        private class QuartzJobStub : IJob
        {
            public QuartzJobStub(Action<IJobExecutionContext> jobAction)
            {
                _jobAction = jobAction;
            }

            private readonly Action<IJobExecutionContext> _jobAction;

            public Task Execute(IJobExecutionContext context)
            {
                _jobAction(context);

                return Task.FromResult(true);
            }
        }


        private class QuartzJobFactoryStub : IJobFactory
        {
            public QuartzJobFactoryStub(Action<IJobExecutionContext> jobAction)
            {
                _jobAction = jobAction;
            }

            private readonly Action<IJobExecutionContext> _jobAction;

            public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
            {
                return new QuartzJobStub(_jobAction);
            }

            public void ReturnJob(IJob job)
            {
            }
        }


        private class QuartzLogProviderStub : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (logLevel, messageFunc, exception, formatParameters) => true;
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotSupportedException();
            }

            public IDisposable OpenMappedContext(string key, string value)
            {
                throw new NotSupportedException();
            }
        }


        private class ValueContainer<T>
        {
            public ValueContainer(T value = default(T))
            {
                Value = value;
            }

            public T Value { get; set; }
        }
    }
}