using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Hosting;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
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

            builder.RegisterFactory(GetRabbitMqConnection)
                   .As<RabbitMqManager>()
                   .SingleInstance();

            builder.RegisterType<MessageConsumersStartupInitializer>()
                   .As<IApplicationEventHandler>()
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
        }

        private static RabbitMqConnectionSettings GetRabbitMqConnectionSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<RabbitMqConnectionSettings>(RabbitMqConnectionSettings.SectionName);
        }

        private static RabbitMqManager GetRabbitMqConnection(IContainerResolver resolver)
        {
            return new RabbitMqManager(resolver.Resolve<RabbitMqConnectionSettings>(), resolver.Resolve<IAppEnvironment>().Name);
        }
    }
}