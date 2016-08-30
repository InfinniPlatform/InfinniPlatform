using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Scheduler.Hosting;
using InfinniPlatform.Scheduler.Implementation;
using InfinniPlatform.Scheduler.Metadata;
using InfinniPlatform.Scheduler.Quartz;
using InfinniPlatform.Scheduler.Storage;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Metadata.Documents;

using Quartz;
using Quartz.Logging;
using Quartz.Spi;

namespace InfinniPlatform.Scheduler.IoC
{
    /// <summary>
    /// Модуль регистрации зависимостей <see cref="InfinniPlatform.Scheduler"/>.
    /// </summary>
    internal class SchedulerContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Сериализатор типов обработчиков заданий
            builder.RegisterType<JobHandlerTypeSerializer>()
                   .As<IJobHandlerTypeSerializer>()
                   .SingleInstance();

            // Фабрика для создания информации о задании
            builder.RegisterType<JobInfoFactory>()
                   .As<IJobInfoFactory>()
                   .SingleInstance();

            // Документы планировщика заданий
            builder.RegisterType<SchedulerDocumentMetadataSource>()
                   .As<IDocumentMetadataSource>()
                   .SingleInstance();

            // Хранилище планировщика заданий
            builder.RegisterType<JobInfoRepository>()
                   .As<IJobInfoRepository>()
                   .SingleInstance();

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
                   .As<IAppEventHandler>()
                   .SingleInstance();
        }
    }
}