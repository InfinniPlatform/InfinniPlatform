using InfinniPlatform.MessageQueue;
using InfinniPlatform.MessageQueue.IoC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace

namespace InfinniPlatform.AspNetCore
{
    /// <summary>
    /// Extension methods for RabbitMQ message queue dependencies registration.
    /// </summary>
    public static class RabbitMqMessageQueueExtensions
    {
        /// <summary>
        /// Register RabbitMQ message queue dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddRabbitMqMessageQueue(this IServiceCollection services)
        {
            var options = RabbitMqMessageQueueOptions.Default;

            return AddRabbitMqMessageQueue(services, options);
        }

        /// <summary>
        /// Register RabbitMQ message queue dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="configuration">Configuration properties set.</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddRabbitMqMessageQueue(this IServiceCollection services, IConfiguration configuration)
        {
            var options = configuration.GetSection(RabbitMqMessageQueueOptions.SectionName).Get<RabbitMqMessageQueueOptions>();

            return AddRabbitMqMessageQueue(services, options);
        }

        /// <summary>
        /// Register RabbitMQ message queue dependencies.
        /// </summary>
        /// <param name="services">Collection of registered services.</param>
        /// <param name="options">RabbitMQ message queue options</param>
        /// <returns>Service collection for further services registration.</returns>
        public static IServiceCollection AddRabbitMqMessageQueue(this IServiceCollection services, RabbitMqMessageQueueOptions options)
        {
            return services.AddSingleton(provider => new RabbitMqMessageQueueContainerModule(options ?? RabbitMqMessageQueueOptions.Default));
        }
    }
}