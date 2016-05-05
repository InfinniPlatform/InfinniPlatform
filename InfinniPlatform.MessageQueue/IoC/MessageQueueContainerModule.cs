using InfinniPlatform.MessageQueue.RabbitMQNew;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.MessageQueue.IoC
{
    public class MessageQueueContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
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
    }
}