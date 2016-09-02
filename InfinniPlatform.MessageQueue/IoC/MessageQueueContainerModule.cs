using InfinniPlatform.Core.Diagnostics;
using InfinniPlatform.MessageQueue.Diagnostics;
using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Hosting;
using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.MessageQueue.RabbitMq.Management.HttpAPI;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;
using InfinniPlatform.Sdk.Queues.Producers;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.MessageQueue.IoC
{
    public class MessageQueueContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterFactory(GetRabbitMqConnectionSettings)
                   .As<RabbitMqConnectionSettings>()
                   .SingleInstance();

            builder.RegisterType<RabbitMqManager>()
                   .As<RabbitMqManager>()
                   .SingleInstance();

            builder.RegisterType<RabbitMqManagementHttpClient>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<MessageConsumersStartupInitializer>()
                   .As<IAppEventHandler>()
                   .SingleInstance();

            builder.RegisterType<OnDemandConsumer>()
                   .As<IOnDemandConsumer>()
                   .SingleInstance();

            builder.RegisterType<TaskProducer>()
                   .As<ITaskProducer>()
                   .SingleInstance();

            builder.RegisterType<BroadcastProducer>()
                   .As<IBroadcastProducer>()
                   .SingleInstance();

            builder.RegisterType<MessageSerializer>()
                   .As<IMessageSerializer>()
                   .SingleInstance();

            builder.RegisterType<MessageConsumerSource>()
                   .As<IMessageConsumerSource>()
                   .SingleInstance();

            // Diagnostics

            builder.RegisterType<MessageQueueStatusProvider>()
                   .As<ISubsystemStatusProvider>()
                   .SingleInstance();
        }

        private static RabbitMqConnectionSettings GetRabbitMqConnectionSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<RabbitMqConnectionSettings>(RabbitMqConnectionSettings.SectionName);
        }
    }
}