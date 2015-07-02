using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Index;

namespace InfinniPlatform.Api.Metadata
{
	public interface IConfigurationObject
	{
		/// <summary>
		///   Предоставить провайдер версий документа для работы в прикладных скриптах
		/// </summary>
		/// <param name="metadata">метаданные объекта</param>
		/// <param name="configVersion">Версия конфигурации</param>
        /// <param name="tenantId"></param>
		/// <returns>Провайдер версий документа</returns>
        IVersionProvider GetDocumentProviderForType(string metadata, string configVersion, string tenantId);

		/// <summary>
		///   Предоставить провайдер версий документа для работы в прикладных скриптах.
		///   Создает провайдер, возвращающий всегда все версии всех найденных документов
		/// </summary>
		/// <param name="metadata">метаданные объекта</param>
        /// <param name="tenantId"></param>
		/// <returns>Провайдер версий документа</returns>
        IVersionProvider GetDocumentProviderForType(string metadata, string tenantId);


		/// <summary>
		///   Предоставить провайдер версий документа для работы в прикладных скриптах
		///   Создает провайдер, возвращающий версии документов всех существующих в индексе типов
		/// </summary>
		/// <param name="metadata">метаданные объекта</param>
		/// <param name="configVersion">Версия конфигурации</param>
        /// <param name="tenantId"></param>
		/// <returns>Провайдер версий документа</returns>
        IVersionProvider GetDocumentProvider(string metadata, string configVersion, string tenantId);

		/// <summary>
		///   Предоставить провайдер версий документа для работы в прикладных скриптах.
		///   Создает провайдер, возвращающий всегда все версии всех найденных документов всех существующих в индексе типов
		/// </summary>
		/// <param name="metadata">метаданные объекта</param>
        /// <param name="tenantId"></param>
		/// <returns>Провайдер версий документа</returns>
        IVersionProvider GetDocumentProvider(string metadata, string tenantId);

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
