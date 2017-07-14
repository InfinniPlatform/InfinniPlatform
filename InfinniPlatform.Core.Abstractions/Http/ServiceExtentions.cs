using System.Collections.Generic;
using System.Net;
using System.Reflection;
using InfinniPlatform.IoC;

namespace InfinniPlatform.Http
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
                t => typeof(IHttpService).GetTypeInfo().IsAssignableFrom(t),
                r => r.As<IHttpService>().SingleInstance());
        }

        /// <summary>
        /// Устанавливает заголовок 'Content-Disposition' типа 'inline' (файл нужно отобразить средствами браузера).
        /// </summary>
        /// <param name="response">Ответ.</param>
        public static void SetContentDispositionInline(this StreamHttpResponse response)
        {
            SetContentDisposition(response, DispositionTypeInline, response.FileName);
        }

        /// <summary>
        /// Устанавливает заголовок 'Content-Disposition' типа 'inline' (файл нужно отобразить средствами браузера).
        /// </summary>
        /// <param name="response">Ответ.</param>
        /// <param name="fileName">Имя файла.</param>
        public static void SetContentDispositionInline(this IHttpResponse response, string fileName)
        {
            SetContentDisposition(response, DispositionTypeInline, fileName);
        }

        /// <summary>
        /// Устанавливает заголовок 'Content-Disposition' типа 'attachment' (файл нужно скачать и не отображать средствами браузера).
        /// </summary>
        /// <param name="response">Ответ.</param>
        public static void SetContentDispositionAttachment(this StreamHttpResponse response)
        {
            SetContentDisposition(response, DispositionTypeAttachment, response.FileName);
        }

        /// <summary>
        /// Устанавливает заголовок 'Content-Disposition' типа 'attachment' (файл нужно скачать и не отображать средствами браузера).
        /// </summary>
        /// <param name="response">Ответ.</param>
        /// <param name="fileName">Имя файла.</param>
        public static void SetContentDispositionAttachment(this IHttpResponse response, string fileName)
        {
            SetContentDisposition(response, DispositionTypeAttachment, fileName);
        }


        private static void SetContentDisposition(this IHttpResponse response, string dispositionType, string fileName)
        {
            if (response.Headers == null)
            {
                response.Headers = new Dictionary<string, string>();
            }

            response.Headers["Content-Disposition"] = $"{dispositionType}; filename=\"{WebUtility.UrlEncode(fileName)}\"";
        }
    }
}