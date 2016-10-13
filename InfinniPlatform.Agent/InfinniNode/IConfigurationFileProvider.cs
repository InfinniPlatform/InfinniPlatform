using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Agent.InfinniNode
{
    /// <summary>
    /// Предоставляет доступ к конфигурационным файлам.
    /// </summary>
    public interface IConfigurationFileProvider
    {
        /// <summary>
        /// Возвращает содержимое файла конфигурации.
        /// </summary>
        /// <param name="appFullName">Полное наименование приложения (содержит имя, версию, имя экземпляра).</param>
        /// <param name="fileName">Имя файла конфигурации.</param>
        StreamHttpResponse Get(string appFullName, string fileName);

        /// <summary>
        /// </summary>
        /// <param name="appFullName">Полное наименование приложения (содержит имя, версию, имя экземпляра).</param>
        /// <param name="fileName">Имя файла конфигурации.</param>
        /// <param name="content">Содержимое файла конфигурации.</param>
        void Set(string appFullName, string fileName, string content);
    }
}