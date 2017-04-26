using InfinniPlatform.Diagnostics;
using InfinniPlatform.Hosting;
using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue.Diagnostics;
using InfinniPlatform.MessageQueue.Hosting;
using InfinniPlatform.MessageQueue.Management;

namespace InfinniPlatform.MessageQueue.IoC
{
    public class RabbitMqMessageQueueContainerModule : IContainerModule
    {
        public RabbitMqMessageQueueContainerModule(RabbitMqMessageQueueOptions options)
        {
            _options = options;
        }

        private readonly RabbitMqMessageQueueOptions _options;

        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            // Hosting

            builder.RegisterType<RabbitMqMessageQueueInitializer>()
                   .As<IAppStartedHandler>()
                   .As<IAppStoppedHandler>()
                   .SingleInstance();

            builder.RegisterType<RabbitMqManager>()
                   .As<RabbitMqManager>()
                   .SingleInstance();

            builder.RegisterType<RabbitMqManagementHttpClient>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<RabbitMqMessageQueueConsumersManager>()
                   .As<IMessageQueueConsumersManager>()
                   .SingleInstance();

            builder.RegisterType<MessageQueueThreadPool>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<DefaultConsumerSource>()
                   .As<IConsumerSource>()
                   .SingleInstance();

            // Producers

            builder.RegisterType<RabbitMqOnDemandConsumer>()
                   .As<IOnDemandConsumer>()
                   .SingleInstance();

            builder.RegisterType<RabbitMqTaskProducer>()
                   .As<ITaskProducer>()
                   .SingleInstance();

            builder.RegisterType<RabbitMqBroadcastProducer>()
                   .As<IBroadcastProducer>()
                   .SingleInstance();

            // Diagnostics

            builder.RegisterType<RabbitMqMessageQueueStatusProvider>()
                   .As<ISubsystemStatusProvider>()
                   .SingleInstance();

            // Other

            builder.RegisterType<RabbitMqMessageSerializer>()
                   .As<IRabbitMqMessageSerializer>()
                   .SingleInstance();

            builder.RegisterType<RabbitMqBasicPropertiesProvider>()
                   .As<IRabbitMqBasicPropertiesProvider>()
                   .SingleInstance();
        }
    }
}