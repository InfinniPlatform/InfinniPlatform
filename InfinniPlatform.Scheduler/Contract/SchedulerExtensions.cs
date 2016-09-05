using System.Reflection;

using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Константы и методы расширения для <see cref="InfinniPlatform.Scheduler"/>.
    /// </summary>
    public static class SchedulerExtensions
    {
        /// <summary>
        /// Имя компонента.
        /// </summary>
        public const string ComponentName = "Scheduler";

        /// <summary>
        /// Префикс для наименований объектов.
        /// </summary>
        /// <remarks>
        /// Используется в наименовании типов документов хранилища данных
        /// и в наименовании очередей распределенной шины сообщений.
        /// </remarks>
        public const string ObjectNamePrefix = ComponentName + ".";


        /// <summary>
        /// Регистрирует все источники заданий <see cref="IJobInfoSource"/> текущей сборки со стратегией <see cref="IContainerRegistrationRule.SingleInstance"/>.
        /// </summary>
        /// <remarks>
        /// Источники заданий <see cref="IJobInfoSource"/> будут зарегистрированы со стратегией <see cref="IContainerRegistrationRule.SingleInstance"/>.
        /// </remarks>
        /// <example>
        /// <code>
        /// RegisterJobInfoSources(GetType().Assembly)
        /// </code>
        /// </example>
        public static void RegisterJobInfoSources(this IContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly,
                                          t => typeof(IJobInfoSource).IsAssignableFrom(t),
                                          r => r.As<IJobInfoSource>().AsSelf().SingleInstance());
        }

        /// <summary>
        /// Регистрирует все обработчики заданий <see cref="IJobHandler"/> текущей сборки со стратегией <see cref="IContainerRegistrationRule.SingleInstance"/>.
        /// </summary>
        /// <remarks>
        /// Обработчики заданий <see cref="IJobHandler"/> будут зарегистрированы со стратегией <see cref="IContainerRegistrationRule.SingleInstance"/>.
        /// </remarks>
        /// <example>
        /// <code>
        /// RegisterJobHandlers(GetType().Assembly)
        /// </code>
        /// </example>
        public static void RegisterJobHandlers(this IContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly,
                                          t => typeof(IJobHandler).IsAssignableFrom(t),
                                          r => r.As<IJobHandler>().AsSelf().SingleInstance());
        }
    }
}