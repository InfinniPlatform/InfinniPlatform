using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Api.ContextComponents
{
	/// <summary>
	///   Контракт для получения метаданных из контекста
	/// </summary>
	public interface IMetadataComponent
	{
	    /// <summary>
	    ///   Получить метаданные конфигурации
	    /// </summary>
	    /// <param name="version">Версия конфигурации</param>
	    /// <param name="configId">Идентификатор конфигурации</param>
	    /// <param name="objectMetadataId">Идентификатор документа</param>
	    /// <param name="metadataType">Идентификатор типа метаданных</param>
	    /// <param name="metadataName">Идентификатор метаданных</param>
	    /// <returns>Метаданные указанного типа</returns>
	    dynamic GetMetadata(string version, string configId, string objectMetadataId, string metadataType, string metadataName);

	    /// <summary>
	    ///   Получить список всех конфигураций
	    /// </summary>
	    /// <param name="version">Версия конфигурации</param>
	    /// <returns>Список метаданных конфигураций</returns>
	    dynamic GetConfigMetadata(string version);

	    /// <summary>
	    ///   Обновить метаданные конфигурации
	    /// </summary>
	    /// <param name="version">Версия конфигурации</param>
	    /// <param name="configId">Идентификатор конфигурации</param>
	    /// <param name="documentId">Идентификатор документа</param>
	    /// <param name="metadataType">Идентификатор типа метаданных</param>
	    /// <param name="metadataName">Идентификатор изменяемых метаданных</param>
	    void UpdateMetadata(string version, string configId, string documentId, string metadataType, string metadataName);

	    /// <summary>
	    ///   Удалить метаданные конфигурации
	    /// </summary>
	    /// <param name="version">Версия конфигурации</param>
	    /// <param name="configId">Идентификатор конфигурации</param>
	    /// <param name="documentId">Идентификатор документа</param>
	    /// <param name="metadataType">Идентификатор типа метаданных</param>
	    /// <param name="metadataName">Идентификатор удаляемых метаданных</param>
	    void DeleteMetadata(string version, string configId, string documentId, string metadataType, string metadataName);

	    /// <summary>
	    ///   Получить метаданные конфигурации
	    /// </summary>
	    /// <param name="version">Версия конфигурации</param>
	    /// <param name="configId">Идентификатор конфигурации</param>
	    /// <param name="objectMetadataId">Идентификатор документа</param>
	    /// <param name="metadataType">Тип метаданных</param>
	    /// <param name="predicate">Предикат для получения метаданных</param>
	    dynamic GetMetadataItem(string version, string configId, string objectMetadataId, string metadataType, Func<object, bool> predicate);

	    /// <summary>
	    ///   Получить метаданные первого уровня вложенности (регистр)
	    /// </summary>
	    /// <param name="version">Версия конфигурации</param>
	    /// <param name="configId">Идентификатор конфигурации</param>
	    /// <param name="objectMetadataId">Идентификатор объекта метаданных (регистр)</param>
	    /// <param name="metadataType">Тип объекта метаданных (MetadataType.Register)</param>
	    /// <returns>Метаданные объекта первого уровня вложенности</returns>
	    IEnumerable<dynamic> GetMetadataList(string version, string configId, string objectMetadataId, string metadataType);
	}
}
