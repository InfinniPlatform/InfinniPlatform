using InfinniPlatform.Core.Index;

namespace InfinniPlatform.Core.Metadata
{
    public interface IConfigurationObject
    {
        /// <summary>
        /// Метаданные конфигурации
        /// </summary>
        IConfigurationMetadata ConfigurationMetadata { get; }

        /// <summary>
        /// Предоставить провайдер версий документа для работы в прикладных скриптах
        /// Создает провайдер, возвращающий версии документов всех существующих в индексе типов
        /// </summary>
        /// <param name="documentId">метаданные объекта</param>
        /// <returns>Провайдер версий документа</returns>
        IVersionProvider GetDocumentProvider(string documentId);
    }
}