using System.Reflection;

using InfinniPlatform.Diagnostics;
using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.Hosting;
using InfinniPlatform.Http;
using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.Scheduler.Common;
using InfinniPlatform.Scheduler.Diagnostics;
using InfinniPlatform.Scheduler.Dispatcher;
using InfinniPlatform.Scheduler.Hosting;
using InfinniPlatform.Scheduler.Storage;
using InfinniPlatform.Settings;

using Quartz;
using Quartz.Logging;
using Quartz.Spi;

namespace InfinniPlatform.Scheduler.IoC
{
    /// <summary>
    /// Модуль регистрации зависимостей <see cref="InfinniPlatform.Scheduler"/>.
    /// </summary>
    public class QuartzSchedulerContainerModule : IContainerModule
    {
        public QuartzSchedulerContainerModule(QuartzSchedulerOptions options)
        {
            _options = options;
        }

        private readonly QuartzSchedulerOptions _options;

        public void Load(IContainerBuilder builder)
        {
            var schedulerAssembly = GetType().GetTypeInfo().Assembly;

            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            // Common

            // Настройки планировщика заданий
            builder.RegisterFactory(GetSchedulerSettings)
                   .As<QuartzSchedulerOptions>()
                   .SingleInstance();

            // Сериализатор типов обработчиков заданий
            builder.RegisterType<JobHandlerTypeSerializer>()
                   .As<IJobHandlerTypeSerializer>()
                   .SingleInstance();

            // Фабрика для создания информации о задании
            builder.RegisterType<JobInfoFactory>()
                   .As<IJobInfoFactory>()
                   .SingleInstance();

            // Фабрика для создания уникального идентификатора экземпляра задания
            builder.RegisterType<JobInstanceFactory>()
                   .As<IJobInstanceFactory>()
                   .SingleInstance();

            // Планировщик заданий
            builder.RegisterType<JobScheduler>()
                   .As<IJobScheduler>()
                   .SingleInstance();

            // Diagnostics

            // Информация о состоянии планировщика заданий
            builder.RegisterType<SchedulerStatusProvider>()
                   .As<ISubsystemStatusProvider>()
                   .As<IHttpService>()
                   .SingleInstance();

            // Storage

            // Документы планировщика заданий
            builder.RegisterType<SchedulerDocumentMetadataSource>()
                   .As<IDocumentMetadataSource>()
                   .SingleInstance();

            // Список выполненных заданий
            builder.RegisterType<JobInstanceManager>()
                   .As<IJobInstanceManager>()
                   .SingleInstance();

            // Хранилище планировщика заданий
            builder.RegisterType<JobInfoRepository>()
                   .As<IJobInfoRepository>()
                   .SingleInstance();

            // Источник сохраненных заданий
            builder.RegisterType<PersistentJobInfoSource>()
                   .As<IJobInfoSource>()
                   .SingleInstance();

            // Queues

            // Обработчики шины сообщений
            builder.RegisterConsumers(schedulerAssembly);

            // Quartz

            // Обработчик заданий Quartz
            builder.RegisterType<QuartzJob>()
                   .As<IJob>()
                   .AsSelf()
                   .SingleInstance();

            // Сервис логирования Quartz
            builder.RegisterType<QuartzJobLogProvider>()
                   .As<ILogProvider>()
                   .SingleInstance();

            // Фабрика обработчиков заданий Quartz
            builder.RegisterType<QuartzJobFactory>()
                   .As<IJobFactory>()
                   .SingleInstance();

            // Диспетчер планировщика заданий Quartz
            builder.RegisterType<QuartzJobSchedulerDispatcher>()
                   .As<IJobSchedulerDispatcher>()
                   .SingleInstance();

            // Hosting

            // Инициализатор планировщика заданий
            builder.RegisterType<SchedulerInitializer>()
                   .As<IAppStartedHandler>()
                   .As<IAppStoppedHandler>()
                   .SingleInstance();

            builder.RegisterJobHandlers(schedulerAssembly);
        }


        private static QuartzSchedulerOptions GetSchedulerSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<QuartzSchedulerOptions>(QuartzSchedulerOptions.SectionName);
        }
    }
}