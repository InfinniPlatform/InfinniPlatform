using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;

namespace InfinniPlatform.Api.Metadata
{
	public interface IConfigurationObject
	{
	    /// <summary>
	    ///   Предоставить провайдер версий документа для работы в прикладных скриптах
	    ///   Создает провайдер, возвращающий версии документов всех существующих в индексе типов
	    /// </summary>
	    /// <param name="metadata">метаданные объекта</param>
	    /// <param name="version"></param>
	    /// <param name="routing"></param>
	    /// <returns>Провайдер версий документа</returns>
	    IVersionProvider GetDocumentProvider(string metadata, string version, string routing);

		/// <summary>
		///   Предоставить провайдер версий документа для работы в прикладных скриптах.
		///   Создает провайдер, возвращающий всегда все версии всех найденных документов всех существующих в индексе типов
		/// </summary>
		/// <param name="metadata">метаданные объекта</param>
		/// <param name="routing"></param>
		/// <returns>Провайдер версий документа</returns>
		IVersionProvider GetDocumentProvider(string metadata, string routing);

		/// <summary>
		///   Получить хранилище бинарных данных
		/// </summary>
		/// <returns>Хранилище бинарных данных</returns>
		IBlobStorage GetBlobStorage();

        /// <summary>
        ///   Получить версию метаданных конфигурации
        /// </summary>
        /// <returns>Версия конфигурации</returns>
	    string GetConfigurationVersion();

	    /// <summary>
	    ///   Получить конструктор версий индекса
	    /// </summary>
	    /// <param name="metadata">Метаданные</param>
	    /// <returns>Конструктор версий</returns>
	    IVersionBuilder GetVersionBuilder(string metadata);

	    /// <summary>
	    ///   Получить идентификатор конфигурации метаданных
	    /// </summary>
	    /// <returns>Идентификатор конфигурации метаданных</returns>
	    string GetConfigurationIdentifier();

        /// <summary>
        ///   Метаданные конфигурации
        /// </summary>
	    IMetadataConfiguration MetadataConfiguration { get; }
	}
}
