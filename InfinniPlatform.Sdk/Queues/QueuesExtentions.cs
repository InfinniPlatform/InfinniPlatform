using System.Reflection;

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
        /// RegisterHttpServices(GetType().Assembly)
        /// </code>
        /// </example>
        public static void RegisterEventingConsumers(this IContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly,
                                          t => typeof(IEventingConsumer).IsAssignableFrom(t),
                                          r => r.As<IEventingConsumer>().SingleInstance());
        }
    }
}