using System;
using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.Sdk.Services
{
    /// <summary>
    /// Ответ.
    /// </summary>
    public class HttpResponse : IHttpResponse
    {
        /// <summary>
        /// Запрос выполнен успешно.
        /// </summary>
        public static readonly IHttpResponse Ok = new HttpResponse();

        /// <summary>
        /// Запрашиваемый ресурс не найден.
        /// </summary>
        public static readonly IHttpResponse NotFound = new HttpResponse(404);

        /// <summary>
        /// Ответ не имеет содержимого.
        /// </summary>
        public static readonly Action<Stream> NoContent = responseStream => { };


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
    }
}