using InfinniPlatform.Core.Diagnostics;
using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.IoC;
using InfinniPlatform.Core.Settings;
using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;
using InfinniPlatform.MessageQueue.Abstractions.Producers;
using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.MessageQueue.RabbitMq.Management.HttpAPI;
using InfinniPlatform.MessageQueue.RabbitMQ.Diagnostics;
using InfinniPlatform.MessageQueue.RabbitMQ.Hosting;
using InfinniPlatform.MessageQueue.RabbitMQ.Serialization;

namespace InfinniPlatform.MessageQueue.RabbitMQ.IoC
{
    public class MessageQueueContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Hosting

            builder.RegisterType<MessageQueueInitializer>()
                   .As<IAppStartedHandler>()
                   .As<IAppStoppedHandler>()
                   .SingleInstance();

            builder.RegisterFactory(GetRabbitMqConnectionSettings)
                   .As<RabbitMqConnectionSettings>()
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
                   .As<IMessageQueueThreadPool>()
                   .SingleInstance();

            builder.RegisterType<MessageConsumeEventHandler>()
                   .As<IMessageConsumeEventHandler>()
                   .SingleInstance();

            builder.RegisterType<MessageConsumerSource>()
                   .As<IMessageConsumerSource>()
                   .SingleInstance();

            // Producers

            builder.RegisterType<OnDemandConsumer>()
                   .As<IOnDemandConsumer>()
                   .SingleInstance();

            builder.RegisterType<TaskProducer>()
                   .As<ITaskProducer>()
                   .SingleInstance();

            builder.RegisterType<BroadcastProducer>()
                   .As<IBroadcastProducer>()
                   .SingleInstance();

            // Diagnostics

            builder.RegisterType<MessageQueueStatusProvider>()
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

        private static RabbitMqConnectionSettings GetRabbitMqConnectionSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<RabbitMqConnectionSettings>(RabbitMqConnectionSettings.SectionName);
        }
    }
}