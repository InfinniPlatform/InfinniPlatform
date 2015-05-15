using System.Collections.Generic;
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

		public dynamic GetSchema(string configId, string documentId)
		{
			IEnumerable<dynamic> schemaMetadata = _metadataComponent.GetMetadataList(configId, documentId,MetadataType.Schema);
			return schemaMetadata.FirstOrDefault();
		}
	}
}
