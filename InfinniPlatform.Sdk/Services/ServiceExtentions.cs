using System.Reflection;

using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Sdk.Services
{
    public static class ServiceExtentions
    {
        /// <summary>
        /// Регистрирует все прикладные сервисы текущей сборки.
        /// </summary>
        /// <remarks>
        /// Прикладные скрипты будут зарегистрированы со стратегией SingleInstance().
        /// </remarks>
        /// <example>
        /// <code>
        /// RegisterHttpServices(GetType().Assembly)
        /// </code>
        /// </example>
        public static void RegisterHttpServices(this IContainerBuilder builder, Assembly assembly)
        {
            builder.RegisterAssemblyTypes(assembly,
                t => typeof(IHttpService).IsAssignableFrom(t),
                r => r.As<IHttpService>().SingleInstance());
        }

        /// <summary>
        /// Устанавливает обработчик запросов.
        /// </summary>
        /// <example>
        /// <code>
        /// Action("/method", new THandler());
        /// </code>
        /// </example>
        public static IHttpServiceRouteBuilder Action<THandler>(this IHttpServiceRouteBuilder target, string path, THandler handler) where THandler : IHttpRequestHandler
        {
            target[path] = handler.Action;
            return target;
        }
    }
}