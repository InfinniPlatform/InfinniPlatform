using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;

using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Security;

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


        /// <summary>
        /// Устанавливает необходимость аутентификации пользователя.
        /// </summary>
        /// <remarks>
        /// Если пользователь не идентифицирован, клиенту будет возвращен ответ <see cref="HttpResponse.Unauthorized"/>.
        /// </remarks>
        public static void RequiresAuthentication(this IHttpServiceBuilder target)
        {
            target.OnBefore += request => (request.User == null || !request.User.IsAuthenticated) ? HttpResponse.Unauthorized : null;
        }

        /// <summary>
        /// Устанавливает необходимость наличия у пользователя заданного типа утверждения.
        /// </summary>
        /// <remarks>
        /// Если пользователь не идентифицирован, клиенту будет возвращен ответ <see cref="HttpResponse.Unauthorized"/>.
        /// Если пользователь не имеет утверждения заданного типа, клиенту будет возвращен ответ <see cref="HttpResponse.Forbidden"/>.
        /// </remarks>
        public static void RequiresClaim(this IHttpServiceBuilder target, string claimType)
        {
            target.OnBefore += request
                => (request.User == null || !request.User.IsAuthenticated)
                    ? HttpResponse.Unauthorized
                    : !request.User.HasClaim(claimType)
                        ? HttpResponse.Forbidden
                        : null;
        }

        /// <summary>
        /// Устанавливает необходимость наличия у пользователя заданного типа утверждения с заданным значением.
        /// </summary>
        /// <remarks>
        /// Если пользователь не идентифицирован, клиенту будет возвращен ответ <see cref="HttpResponse.Unauthorized"/>.
        /// Если пользователь не имеет утверждения заданного типа с заданным значением, клиенту будет возвращен ответ <see cref="HttpResponse.Forbidden"/>.
        /// </remarks>
        public static void RequiresClaim(this IHttpServiceBuilder target, string claimType, string claimValue)
        {
            target.OnBefore += request
                => (request.User == null || !request.User.IsAuthenticated)
                    ? HttpResponse.Unauthorized
                    : !request.User.HasClaim(claimType, claimValue)
                        ? HttpResponse.Forbidden
                        : null;
        }

        /// <summary>
        /// Устанавливает необходимость наличия у пользователя всех заданных типов утверждений.
        /// </summary>
        /// <remarks>
        /// Если пользователь не идентифицирован, клиенту будет возвращен ответ <see cref="HttpResponse.Unauthorized"/>.
        /// Если пользователь не имеет всех утверждений заданных типов, клиенту будет возвращен ответ <see cref="HttpResponse.Forbidden"/>.
        /// </remarks>
        public static void RequiresAllClaims(this IHttpServiceBuilder target, IEnumerable<string> claimTypes)
        {
            target.OnBefore += request
                => (request.User == null || !request.User.IsAuthenticated)
                    ? HttpResponse.Unauthorized
                    : !request.User.HasAllClaims(claimTypes)
                        ? HttpResponse.Forbidden
                        : null;
        }

        /// <summary>
        /// Устанавливает необходимость наличия у пользователя всех заданных типов утверждений.
        /// </summary>
        /// <remarks>
        /// Если пользователь не идентифицирован, клиенту будет возвращен ответ <see cref="HttpResponse.Unauthorized"/>.
        /// Если пользователь не имеет всех утверждений заданных типов, клиенту будет возвращен ответ <see cref="HttpResponse.Forbidden"/>.
        /// </remarks>
        public static void RequiresAllClaims(this IHttpServiceBuilder target, params string[] claimTypes)
        {
            RequiresAllClaims(target, (IEnumerable<string>)claimTypes);
        }

        /// <summary>
        /// Устанавливает необходимость наличия у пользователя одного из заданных типов утверждений.
        /// </summary>
        /// <remarks>
        /// Если пользователь не идентифицирован, клиенту будет возвращен ответ <see cref="HttpResponse.Unauthorized"/>.
        /// Если пользователь не имеет ни одного утверждения из заданных типов, клиенту будет возвращен ответ <see cref="HttpResponse.Forbidden"/>.
        /// </remarks>
        public static void RequiresAnyClaims(this IHttpServiceBuilder target, IEnumerable<string> claimTypes)
        {
            target.OnBefore += request
                => (request.User == null || !request.User.IsAuthenticated)
                    ? HttpResponse.Unauthorized
                    : !request.User.HasAnyClaims(claimTypes)
                        ? HttpResponse.Forbidden
                        : null;
        }

        /// <summary>
        /// Устанавливает необходимость наличия у пользователя одного из заданных типов утверждений.
        /// </summary>
        /// <remarks>
        /// Если пользователь не идентифицирован, клиенту будет возвращен ответ <see cref="HttpResponse.Unauthorized"/>.
        /// Если пользователь не имеет ни одного утверждения из заданных типов, клиенту будет возвращен ответ <see cref="HttpResponse.Forbidden"/>.
        /// </remarks>
        public static void RequiresAnyClaims(this IHttpServiceBuilder target, params string[] claimTypes)
        {
            RequiresAnyClaims(target, (IEnumerable<string>)claimTypes);
        }

        /// <summary>
        /// Устанавливает необходимость наличия пользователя, который проходит заданную проверку.
        /// </summary>
        /// <remarks>
        /// Если пользователь не идентифицирован, клиенту будет возвращен ответ <see cref="HttpResponse.Unauthorized"/>.
        /// Если пользователь не проходит заданную проверку, клиенту будет возвращен ответ <see cref="HttpResponse.Forbidden"/>.
        /// </remarks>
        public static void RequiresValidUser(this IHttpServiceBuilder target, Func<IIdentity, bool> userMatch)
        {
            target.OnBefore += request
                => (request.User == null || !request.User.IsAuthenticated)
                    ? HttpResponse.Unauthorized
                    : !userMatch(request.User)
                        ? HttpResponse.Forbidden
                        : null;
        }
    }
}