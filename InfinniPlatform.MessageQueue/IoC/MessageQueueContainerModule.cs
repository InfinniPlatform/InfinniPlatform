﻿using InfinniPlatform.MessageQueue.RabbitMQNew;
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
                   .As<RabbitMqManager>()
                   .SingleInstance();

            builder.RegisterType<Producer>()
                   .As<IProducer>()
                   .InstancePerDependency();

            builder.RegisterType<BasicConsumer>()
                   .As<IBasicConsumer>()
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

        private static RabbitMqManager GetRabbitMqConnection(IContainerResolver resolver)
        {
            return new RabbitMqManager(resolver.Resolve<RabbitMqConnectionSettings>());
        }
    }
}