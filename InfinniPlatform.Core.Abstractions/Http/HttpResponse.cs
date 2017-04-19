using System;
using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.Core.Http
{
    /// <summary>
    /// Ответ.
    /// </summary>
    public class HttpResponse : IHttpResponse
    {
        /// <summary>
        /// Ответ не имеет содержимого.
        /// </summary>
        public static readonly Action<Stream> NoContent;

        /// <summary>
        /// Запрос успешен.
        /// </summary>
        public static readonly IHttpResponse Ok;

        /// <summary>
        /// Ресурс не найден.
        /// </summary>
        public static readonly IHttpResponse NotFound;

        /// <summary>
        /// Запрос не авторизован.
        /// </summary>
        public static readonly IHttpResponse Unauthorized;

        /// <summary>
        /// Запрос запрещен.
        /// </summary>
        public static readonly IHttpResponse Forbidden;


        /// <summary>
        /// Конструктор.
        /// </summary>
        static HttpResponse()
        {
            NoContent = responseStream => { };

            Ok = new HttpResponse();
            NotFound = new HttpResponse(404);
            Unauthorized = new HttpResponse(401);
            Forbidden = new HttpResponse(403);
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="statusCode">Код состояния.</param>
        /// <param name="contentType">Тип содержимого тела ответа.</param>
        public HttpResponse(int statusCode = 200, string contentType = HttpConstants.JsonContentType)
        {
            StatusCode = statusCode;
            ContentType = contentType;
            Content = NoContent;
        }


        /// <summary>
        /// Код состояния.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Описание состояния.
        /// </summary>
        public string ReasonPhrase { get; set; }

        /// <summary>
        /// Заголовок ответа.
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Тип содержимого тела ответа.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Метод записи содержимого тела ответа.
        /// </summary>
        public Action<Stream> Content { get; set; }


        /// <summary>
        /// Возвращает значение заголовка.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public string GetHeader(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            string value;

            return (Headers != null && Headers.TryGetValue(key, out value)) ? value : null;
        }

        /// <summary>
        /// Устанавливает значение заголовка.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetHeader(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (Headers == null)
            {
                Headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            Headers[key] = value;
        }
    }
}