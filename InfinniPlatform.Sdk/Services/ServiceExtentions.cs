using System;
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
        /// Action("/method", request => 200);
        /// </code>
        /// </example>
        public static IHttpRouteBuilder Action(this IHttpRouteBuilder target, string path, Func<IHttpRequest, object> action)
        {
            return Action(target, path, new LambdaHttpRequestHandler(action));
        }

        /// <summary>
        /// Устанавливает обработчик запросов.
        /// </summary>
        /// <example>
        /// <code>
        /// Action("/method", new THandler());
        /// </code>
        /// </example>
        public static IHttpRouteBuilder Action<THandler>(this IHttpRouteBuilder target, string path, THandler handler) where THandler : IHttpRequestHandler
        {
            target[path] = handler.Action;
            return target;
        }


        private class LambdaHttpRequestHandler : SimpleHttpRequestHandler
        {
            public LambdaHttpRequestHandler(Func<IHttpRequest, object> action)
            {
                _action = action;
            }

            private readonly Func<IHttpRequest, object> _action;

            protected override object ActionResult(IHttpRequest request)
            {
                return _action(request);
            }
        }
    }
}