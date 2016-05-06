using InfinniPlatform.MessageQueue.RabbitMQNew;
using InfinniPlatform.Sdk.IoC;
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
                   .As<RabbitMqConnection>()
                   .SingleInstance();

            builder.RegisterType<Producer>()
                   .As<IProducer>()
                   .InstancePerDependency();

            builder.RegisterType<QueningConsumer>()
                   .As<IQueningConsumer>()
                   .InstancePerDependency();

            builder.RegisterType<EventingConsumer>()
                   .As<IEventingConsumer>()
                   .InstancePerDependency();

            builder.RegisterHttpServices(GetType().Assembly);
        }

        private static RabbitMqConnectionSettings GetRabbitMqConnectionSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<RabbitMqConnectionSettings>(RabbitMqConnectionSettings.SectionName);
        }

        private static RabbitMqConnection GetRabbitMqConnection(IContainerResolver resolver)
        {
            return new RabbitMqConnection(resolver.Resolve<RabbitMqConnectionSettings>());
        }
    }
}