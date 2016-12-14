using System.IO;

namespace InfinniPlatform.DocumentStorage.Contract.RestApi
{
    /// <summary>
    /// Информация о файле запроса.
    /// </summary>
    public sealed class HttpRequestFileClient
    {
        /// <summary>
        /// Тип содержимого файла.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Наименование файла.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Элемент формы.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Поток с файлом.
        /// </summary>
        public Stream Value { get; set; }
    }
}