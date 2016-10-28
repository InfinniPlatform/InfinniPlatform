using System;
using System.Collections.Generic;
using System.IO;

namespace InfinniPlatform.Sdk.Http.Services
{
    /// <summary>
    /// Ответ.
    /// </summary>
    public interface IHttpResponse
    {
        /// <summary>
        /// Код состояния.
        /// </summary>
        int StatusCode { get; set; }

        /// <summary>
        /// Описание состояния.
        /// </summary>
        string ReasonPhrase { get; set; }

        /// <summary>
        /// Заголовок ответа.
        /// </summary>
        IDictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Тип содержимого тела ответа.
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// Содержимое тела ответа.
        /// </summary>
        Action<Stream> Content { get; set; }
    }
}