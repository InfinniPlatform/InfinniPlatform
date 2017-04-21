using System.IO;

namespace InfinniPlatform.Http
{
    /// <summary>
    /// Информация о файле запроса.
    /// </summary>
    public interface IHttpRequestFile
    {
        /// <summary>
        /// Тип содержимого файла.
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Наименование файла.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Элемент формы.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Поток с файлом.
        /// </summary>
        Stream Value { get; }
    }
}