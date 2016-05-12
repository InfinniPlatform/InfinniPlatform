using System.Reflection;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Services;
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

            builder.RegisterType<MessageConsumersManager>()
                   .As<IApplicationEventHandler>()
                   .SingleInstance();

            builder.RegisterType<BasicConsumer>()
                   .As<IBasicConsumer>()
                   .InstancePerDependency();

            builder.RegisterType<ProducerBase>()
                   .As<IProducer>()
                   .InstancePerDependency();

            builder.RegisterType<MessageSerializer>()
                   .As<IMessageSerializer>()
                   .SingleInstance();

            builder.RegisterHttpServices(GetType().GetTypeInfo().Assembly);
            builder.RegisterConsumers(GetType().GetTypeInfo().Assembly);
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