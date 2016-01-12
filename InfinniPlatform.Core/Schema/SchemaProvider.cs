using System.Linq;

using InfinniPlatform.Sdk.ContextComponents;

namespace InfinniPlatform.Core.Schema
{
    public sealed class SchemaProvider : ISchemaProvider
    {
        private readonly IMetadataComponent _metadataComponent;

        public SchemaProvider(IMetadataComponent metadataComponent)
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
            var schemaMetadata = _metadataComponent.GetMetadataList(configId, documentId, "Schema");
			return schemaMetadata?.FirstOrDefault();
        }
    }
}