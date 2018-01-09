using InfinniPlatform.Diagnostics;
using InfinniPlatform.DocumentStorage.Metadata;
using InfinniPlatform.Hosting;
using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue;
using InfinniPlatform.Scheduler.Clusterization;
using InfinniPlatform.Scheduler.Diagnostics;
using InfinniPlatform.Scheduler.Dispatcher;
using InfinniPlatform.Scheduler.Hosting;
using InfinniPlatform.Scheduler.Repository;

using Quartz;
using Quartz.Logging;
using Quartz.Spi;

namespace InfinniPlatform.Scheduler.IoC
{
    /// <summary>
    /// Dependency registration module for <see cref="InfinniPlatform.Scheduler"/>.
    /// </summary>
    public class QuartzSchedulerContainerModule : IContainerModule
    {
        /// <summary>
        /// Creates new instance of <see cref="QuartzSchedulerContainerModule"/>.
        /// </summary>
        public QuartzSchedulerContainerModule(QuartzSchedulerOptions options)
        {
            _options = options;
        }

        private readonly QuartzSchedulerOptions _options;

        /// <inheritdoc />
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            builder.RegisterType<JobHandlerTypeSerializer>().As<IJobHandlerTypeSerializer>().SingleInstance();
            builder.RegisterType<JobInstanceFactory>().As<IJobInstanceFactory>().SingleInstance();
            builder.RegisterType<JobInfoFactory>().As<IJobInfoFactory>().SingleInstance();
            builder.RegisterType<JobScheduler>().As<IJobScheduler>().SingleInstance();

            // Hosting

            builder.RegisterType<SchedulerInitializer>().As<IAppStartedHandler>().As<IAppStoppedHandler>().SingleInstance();

            // Diagnostics

            builder.RegisterType<SchedulerStatusProvider>().As<ISubsystemStatusProvider>().SingleInstance();

            // Dispatcher

            builder.RegisterType<QuartzJob>().As<IJob>().AsSelf().SingleInstance();
            builder.RegisterType<QuartzJobFactory>().As<IJobFactory>().SingleInstance();
            builder.RegisterType<QuartzJobLogProvider>().As<ILogProvider>().SingleInstance();
            builder.RegisterType<QuartzJobSchedulerDispatcher>().As<IJobSchedulerDispatcher>().SingleInstance();

            // Clusterization

            builder.RegisterType<JobHandlerConsumer>().As<IConsumer>().AsSelf().SingleInstance();
            builder.RegisterType<AddOrUpdateJobConsumer>().As<IConsumer>().SingleInstance();
            builder.RegisterType<DeleteJobConsumer>().As<IConsumer>().SingleInstance();
            builder.RegisterType<PauseJobConsumer>().As<IConsumer>().SingleInstance();
            builder.RegisterType<ResumeJobConsumer>().As<IConsumer>().SingleInstance();

            builder.RegisterType<JobSchedulerStateObserver>().AsSelf().SingleInstance();
            builder.RegisterType<JobSchedulerStateObserverStub>().AsSelf().SingleInstance();
            builder.RegisterFactory(CreateJobSchedulerStateObserver).As<IJobSchedulerStateObserver>().SingleInstance();

            // Repository

            builder.RegisterType<JobSchedulerDocumentMetadataSource>().As<IDocumentMetadataSource>().SingleInstance();
            builder.RegisterType<JobSchedulerRepository>().AsSelf().SingleInstance();
            builder.RegisterType<JobSchedulerRepositoryStub>().AsSelf().SingleInstance();
            builder.RegisterFactory(CreateJobSchedulerRepository).As<IJobSchedulerRepository>().SingleInstance();
            builder.RegisterType<PersistentJobInfoSource>().As<IJobInfoSource>().SingleInstance();
        }


        private IJobSchedulerStateObserver CreateJobSchedulerStateObserver(IContainerResolver resolver)
        {
            var jobSchedulerStateObserver = _options.JobSchedulerStateObserver?.Invoke(resolver);

            if (jobSchedulerStateObserver == null)
            {
                if (JobSchedulerStateObserver.CanBeCreated(resolver))
                {
                    jobSchedulerStateObserver = resolver.Resolve<JobSchedulerStateObserver>();
                }
                else
                {
                    jobSchedulerStateObserver = resolver.Resolve<JobSchedulerStateObserverStub>();
                }
            }

            return jobSchedulerStateObserver;
        }

        private IJobSchedulerRepository CreateJobSchedulerRepository(IContainerResolver resolver)
        {
            var jobSchedulerRepository = _options.JobSchedulerRepository?.Invoke(resolver);

            if (jobSchedulerRepository == null)
            {
                if (JobSchedulerRepository.CanBeCreated(resolver))
                {
                    jobSchedulerRepository = resolver.Resolve<JobSchedulerRepository>();
                }
                else
                {
                    jobSchedulerRepository = resolver.Resolve<JobSchedulerRepositoryStub>();
                }
            }

            return jobSchedulerRepository;
        }
    }
}