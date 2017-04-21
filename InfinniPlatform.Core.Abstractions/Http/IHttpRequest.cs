using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Principal;

namespace InfinniPlatform.Http
{
    /// <summary>
    /// Запрос.
    /// </summary>
    public interface IHttpRequest
    {
        /// <summary>
        /// Метод запроса.
        /// </summary>
        string Method { get; }

        /// <summary>
        /// Базовый путь запроса.
        /// </summary>
        string BasePath { get; }

        /// <summary>
        /// Путь запроса.
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Заголовок запроса.
        /// </summary>
        IHttpRequestHeaders Headers { get; }

        /// <summary>
        /// Параметры строки запроса.
        /// </summary>
        dynamic Parameters { get; }

        /// <summary>
        /// Данные строки запроса.
        /// </summary>
        dynamic Query { get; }

        /// <summary>
        /// Данные тела запроса.
        /// </summary>
        dynamic Form { get; }

        /// <summary>
        /// Список файлов запроса.
        /// </summary>
        IEnumerable<IHttpRequestFile> Files { get; }

        /// <summary>
        /// Содержимое тела запроса.
        /// </summary>
        Stream Content { get; }

        /// <summary>
        /// Пользователь запроса.
        /// </summary>
        IIdentity User { get; }

        /// <summary>
        /// Региональные параметры запроса.
        /// </summary>
        CultureInfo Culture { get; }

        /// <summary>
        /// Адрес клиента.
        /// </summary>
        string UserHostAddress { get; }
    }
}