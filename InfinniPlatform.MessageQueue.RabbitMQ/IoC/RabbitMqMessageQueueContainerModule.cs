using InfinniPlatform.Diagnostics;
using InfinniPlatform.Hosting;
using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue.Diagnostics;
using InfinniPlatform.MessageQueue.Hosting;
using InfinniPlatform.MessageQueue.Management;

namespace InfinniPlatform.MessageQueue.IoC
{
    /// <summary>
    /// Dependency registration module for <see cref="InfinniPlatform.MessageQueue" />.
    /// </summary>
    public class RabbitMqMessageQueueContainerModule : IContainerModule
    {
        private readonly RabbitMqMessageQueueOptions _options;

        /// <summary>
        /// Creates new instance of <see cref="RabbitMqMessageQueueContainerModule"/>.
        /// </summary>
        public RabbitMqMessageQueueContainerModule(RabbitMqMessageQueueOptions options)
        {
            _options = options;
        }

        /// <inheritdoc />
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            // Hosting

            builder.RegisterType<MessageQueueInitializer>()
                .As<IAppStartedHandler>()
                .As<IAppStoppedHandler>()
                .SingleInstance();

            builder.RegisterType<RabbitMqManager>()
                .As<RabbitMqManager>()
                .SingleInstance();

            builder.RegisterType<RabbitMqManagementHttpClient>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<MessageQueueConsumersManager>()
                .As<IMessageQueueConsumersManager>()
                .SingleInstance();

            builder.RegisterType<MessageQueueThreadPool>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<DefaultConsumerSource>()
                .As<IConsumerSource>()
                .SingleInstance();

            // Producers

            builder.RegisterType<TaskProducer>()
                .As<ITaskProducer>()
                .SingleInstance();

            builder.RegisterType<BroadcastProducer>()
                .As<IBroadcastProducer>()
                .SingleInstance();

            // Diagnostics

            builder.RegisterType<RabbitMqMessageQueueStatusProvider>()
                .As<ISubsystemStatusProvider>()
                .SingleInstance();

            // Other

            builder.RegisterType<MessageSerializer>()
                .As<IMessageSerializer>()
                .SingleInstance();

            builder.RegisterType<BasicPropertiesProvider>()
                .As<IBasicPropertiesProvider>()
                .SingleInstance();
        }
    }
}