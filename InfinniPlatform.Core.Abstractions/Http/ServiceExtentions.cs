using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using InfinniPlatform.Core.Abstractions.IoC;

namespace InfinniPlatform.Core.Abstractions.Http
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