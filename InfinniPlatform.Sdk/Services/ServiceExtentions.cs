using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Security;

namespace InfinniPlatform.Sdk.Services
{
    public static class ServiceExtentions
    {
        private const string DispositionTypeInline = "inline";
        private const string DispositionTypeAttachment = "attachment";


        /// <summary>
        /// Регистрирует все прикладные сервисы текущей сборки.
        /// </summary>
        /// <remarks>
        /// Сервисы будут зарегистрированы со стратегией SingleInstance().
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
        /// Устанавливает необходимость аутентификации пользователя.
        /// </summary>
        /// <remarks>
        /// Если пользователь не идентифицирован, клиенту будет возвращен ответ <see cref="HttpResponse.Unauthorized"/>.
        /// </remarks>
        public static void RequiresAuthentication(this IHttpServiceBuilder target)
        {
            target.OnBefore += request => Task.FromResult<object>((request.User == null || !request.User.IsAuthenticated) ? HttpResponse.Unauthorized : null);
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
                => Task.FromResult<object>((request.User == null || !request.User.IsAuthenticated)
                    ? HttpResponse.Unauthorized
                    : !request.User.HasClaim(claimType)
                        ? HttpResponse.Forbidden
                        : null);
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
                => Task.FromResult<object>((request.User == null || !request.User.IsAuthenticated)
                    ? HttpResponse.Unauthorized
                    : !request.User.HasClaim(claimType, claimValue)
                        ? HttpResponse.Forbidden
                        : null);
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
                => Task.FromResult<object>((request.User == null || !request.User.IsAuthenticated)
                    ? HttpResponse.Unauthorized
                    : !request.User.HasAllClaims(claimTypes)
                        ? HttpResponse.Forbidden
                        : null);
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
                => Task.FromResult<object>((request.User == null || !request.User.IsAuthenticated)
                    ? HttpResponse.Unauthorized
                    : !request.User.HasAnyClaims(claimTypes)
                        ? HttpResponse.Forbidden
                        : null);
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
                => Task.FromResult<object>((request.User == null || !request.User.IsAuthenticated)
                    ? HttpResponse.Unauthorized
                    : !userMatch(request.User)
                        ? HttpResponse.Forbidden
                        : null);
        }


        /// <summary>
        /// Устанавливает заголовок 'Content-Disposition' типа 'inline' (файл нужно отобразить средствами браузера).
        /// </summary>
        /// <param name="response">Ответ.</param>
        /// <param name="userAgent">Значение заголовка запроса 'User-Agent'.</param>
        public static void SetContentDispositionInline(this StreamHttpResponse response, string userAgent)
        {
            SetContentDisposition(response, DispositionTypeInline, response.FileName, userAgent);
        }

        /// <summary>
        /// Устанавливает заголовок 'Content-Disposition' типа 'inline' (файл нужно отобразить средствами браузера).
        /// </summary>
        /// <param name="response">Ответ.</param>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="userAgent">Значение заголовка запроса 'User-Agent'.</param>
        public static void SetContentDispositionInline(this IHttpResponse response, string fileName, string userAgent)
        {
            SetContentDisposition(response, DispositionTypeInline, fileName, userAgent);
        }

        /// <summary>
        /// Устанавливает заголовок 'Content-Disposition' типа 'attachment' (файл нужно скачать и не отображать средствами браузера).
        /// </summary>
        /// <param name="response">Ответ.</param>
        /// <param name="userAgent">Значение заголовка запроса 'User-Agent'.</param>
        public static void SetContentDispositionAttachment(this StreamHttpResponse response, string userAgent)
        {
            SetContentDisposition(response, DispositionTypeAttachment, response.FileName, userAgent);
        }

        /// <summary>
        /// Устанавливает заголовок 'Content-Disposition' типа 'attachment' (файл нужно скачать и не отображать средствами браузера).
        /// </summary>
        /// <param name="response">Ответ.</param>
        /// <param name="fileName">Имя файла.</param>
        /// <param name="userAgent">Значение заголовка запроса 'User-Agent'.</param>
        public static void SetContentDispositionAttachment(this IHttpResponse response, string fileName, string userAgent)
        {
            SetContentDisposition(response, DispositionTypeAttachment, fileName, userAgent);
        }


        private static void SetContentDisposition(this IHttpResponse response, string dispositionType, string fileName, string userAgent)
        {
            if (response.Headers == null)
            {
                response.Headers = new Dictionary<string, string>();
            }

            response.Headers["Content-Disposition"] = $"{dispositionType}; filename=\"{EncodeContentDespositionFileName(fileName, userAgent)}\"";
        }

        private static string EncodeContentDespositionFileName(string fileName, string userAgent)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var fileNameEcoding = Encoding.UTF8;

                // Microsoft Internet Explorer or Edge
                if (!string.IsNullOrEmpty(userAgent)
                    && ((userAgent.IndexOf("Edge", StringComparison.OrdinalIgnoreCase) >= 0)
                        || (userAgent.IndexOf("MSIE", StringComparison.OrdinalIgnoreCase) >= 0)
                        || (userAgent.IndexOf("Trident", StringComparison.OrdinalIgnoreCase) >= 0)
                        || (userAgent.IndexOf("rv:11.0", StringComparison.OrdinalIgnoreCase) >= 0)))
                {
                    fileNameEcoding = Encoding.GetEncoding("windows-1251");
                }

                var fileNameBytes = fileNameEcoding.GetBytes(fileName);

                var dispositionFileName = "";

                foreach (var b in fileNameBytes)
                {
                    dispositionFileName += (char)(b & 0xff);
                }

                return dispositionFileName;
            }

            return fileName;
        }
    }
}