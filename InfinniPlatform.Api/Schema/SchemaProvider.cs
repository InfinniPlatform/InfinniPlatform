﻿using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Metadata;

namespace InfinniPlatform.Api.Schema
{
	public sealed class SchemaProvider : ISchemaProvider
	{
		private readonly IMetadataComponent _metadataComponent;

		public SchemaProvider(IMetadataComponent metadataComponent)
		{
			_metadataComponent = metadataComponent;
		}

	    /// <summary>
	    ///  Получить схему документа
	    /// </summary>
	    /// <param name="version">Версия конфигурации</param>
	    /// <param name="configId">Идентификатор конфигурации</param>
	    /// <param name="documentId">Идентификатор документа</param>
	    /// <returns>Схема документа</returns>
	    public dynamic GetSchema(string version, string configId, string documentId)
		{
			IEnumerable<dynamic> schemaMetadata = _metadataComponent.GetMetadataList(version, configId, documentId,MetadataType.Schema);
			return schemaMetadata.FirstOrDefault();
		}
	}
}
