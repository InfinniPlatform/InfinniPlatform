using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.MessageQueue.Contract.Consumers;
using InfinniPlatform.MessageQueue.Contract.Producers;
using InfinniPlatform.MessageQueue.Diagnostics;
using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Hosting;
using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.MessageQueue.RabbitMq.Management.HttpAPI;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Diagnostics;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.MessageQueue.IoC
{
    public class MessageQueueContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Hosting

            builder.RegisterType<MessageQueueInitializer>()
                   .As<IAppEventHandler>()
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