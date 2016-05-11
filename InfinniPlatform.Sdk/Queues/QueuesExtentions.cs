using System.Reflection;

using InfinniPlatform.MessageQueue.RabbitMQNew;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Sdk.Queues
{
    public static class QueuesExtentions
    {
        /// <summary>
        /// Регистрирует всех прикладных потребителей сообщений текущей сборки.
        /// </summary>
        /// <remarks>
        /// Сервисы будут зарегистрированы со стратегией SingleInstance().
        /// </remarks>
        /// <example>
        ///     <code>
        /// RegisterConsumers(GetType().Assembly)
        /// </code>
        /// </example>
        public static void RegisterConsumers(this IContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly,
                                          t => typeof(IConsumer).IsAssignableFrom(t),
                                          r => r.As<IConsumer>().SingleInstance());
        }

        /// <summary>
        /// Регистрирует всех прикладных поставщиков сообщений текущей сборки.
        /// </summary>
        /// <remarks>
        /// Сервисы будут зарегистрированы со стратегией SingleInstance().
        /// </remarks>
        /// <example>
        ///     <code>
        /// RegisterPublishers(GetType().Assembly)
        /// </code>
        /// </example>
        public static void RegisterPublishers(this IContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly,
                                          t => typeof(IProducer).IsAssignableFrom(t),
                                          r => r.As<IProducer>().SingleInstance());
        }
    }
}