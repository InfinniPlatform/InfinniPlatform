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
        public HttpResponse()
        {
            StatusCode = 200;
            ContentType = HttpConstants.JsonContentType;
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
        /// Содержимое тела ответа.
        /// </summary>
        public Action<Stream> Content { get; set; }
    }
}