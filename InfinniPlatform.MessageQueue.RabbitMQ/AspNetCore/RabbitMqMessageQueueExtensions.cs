using InfinniPlatform.MessageQueue;
using InfinniPlatform.MessageQueue.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    public static class RabbitMqMessageQueueExtensions
    {
        public static IServiceCollection AddRabbitMqMessageQueue(this IServiceCollection services)
        {
            var options = RabbitMqMessageQueueOptions.Default;

            return AddRabbitMqMessageQueue(services, options);
        }

        public static IServiceCollection AddRabbitMqMessageQueue(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var options = configuration.GetSection(RabbitMqMessageQueueOptions.SectionName).Get<RabbitMqMessageQueueOptions>();

            return AddRabbitMqMessageQueue(services, options);
        }

        public static IServiceCollection AddRabbitMqMessageQueue(this IServiceCollection services, RabbitMqMessageQueueOptions options)
        {
            return services.AddSingleton(provider => new RabbitMqMessageQueueContainerModule(options ?? RabbitMqMessageQueueOptions.Default));
        }
    }
}