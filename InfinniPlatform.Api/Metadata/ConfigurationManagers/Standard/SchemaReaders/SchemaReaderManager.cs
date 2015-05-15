using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Schema;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.SchemaReaders
{
	public sealed class SchemaReaderManager : ISchemaProvider
	{
		public dynamic GetSchema(string configId, string documentId)
		{
			return new MetadataApi().GetDocumentSchema(configId, documentId);			
		}
	}
}
