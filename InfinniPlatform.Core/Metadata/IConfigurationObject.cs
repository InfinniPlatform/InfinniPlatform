using InfinniPlatform.Sdk.BlobStorage;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Core.Metadata
{
    public interface IConfigurationObject
    {
        /// <summary>
        ///     Метаданные конфигурации
        /// </summary>
        IMetadataConfiguration MetadataConfiguration { get; }

        /// <summary>
        ///     Предоставить провайдер версий документа для работы в прикладных скриптах
        ///     Создает провайдер, возвращающий версии документов всех существующих в индексе типов
        /// </summary>
        /// <param name="documentId">метаданные объекта</param>
        /// <returns>Провайдер версий документа</returns>
        IVersionProvider GetDocumentProvider(string documentId);

        /// <summary>
        ///     Получить хранилище бинарных данных
        /// </summary>
        /// <returns>Хранилище бинарных данных</returns>
        IBlobStorage GetBlobStorage();

        /// <summary>
        ///     Получить версию метаданных конфигурации
        /// </summary>
        /// <returns>Версия конфигурации</returns>
        string GetConfigurationVersion();

        /// <summary>
        ///     Получить конструктор версий индекса
        /// </summary>
        /// <param name="documentId">Метаданные</param>
        /// <returns>Конструктор версий</returns>
        IVersionBuilder GetVersionBuilder(string documentId);

        /// <summary>
        ///     Получить идентификатор конфигурации метаданных
        /// </summary>
        /// <returns>Идентификатор конфигурации метаданных</returns>
        string GetConfigurationIdentifier();
    }
}