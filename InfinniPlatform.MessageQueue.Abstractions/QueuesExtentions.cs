using System.Reflection;

using InfinniPlatform.Core.Abstractions.IoC;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;

namespace InfinniPlatform.MessageQueue.Abstractions
{
    public static class QueuesExtentions
    {
        /// <summary>
        /// Регистрирует всех потребителей сообщений текущей сборки.
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
                                          t => typeof(IConsumer).GetTypeInfo().IsAssignableFrom(t),
                                          r => r.AsImplementedInterfaces().SingleInstance());
        }
    }
}