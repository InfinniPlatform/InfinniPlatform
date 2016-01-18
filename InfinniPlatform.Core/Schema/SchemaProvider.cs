﻿using System.Linq;

using InfinniPlatform.Core.Metadata;

namespace InfinniPlatform.Core.Schema
{
    public sealed class SchemaProvider : ISchemaProvider
    {
        private readonly IMetadataApi _metadataComponent;

        public SchemaProvider(IMetadataApi metadataComponent)
        {
            _metadataComponent = metadataComponent;
        }

        /// <summary>
        ///     Получить схему документа
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="documentId">Идентификатор документа</param>
        /// <returns>Схема документа</returns>
        public dynamic GetSchema(string configId, string documentId)
        {
            return _metadataComponent.GetDocumentSchema(configId, documentId);
        }
    }
}