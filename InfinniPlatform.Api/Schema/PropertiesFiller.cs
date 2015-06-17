using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.SchemaReaders;
using InfinniPlatform.Api.Properties;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.Schema
{
	/// <summary>
	///   Заполнитель схемы метаданных документа
	/// </summary>
	public sealed class PropertiesFiller
	{
		private dynamic GetDocumentSchema(string version, string configuration, string document)
		{
			if (!string.IsNullOrEmpty(configuration) && !string.IsNullOrEmpty(document))
			{
				return new SchemaReaderManager().GetSchema(version, configuration, document);
			}
			return null;
		}

		public IEnumerable<SchemaObject> FillProperties(string version, string configuration, string document, string alias, PathResolveType pathResolveType)
		{
			var propertiesResult = new List<SchemaObject>();
			if (!string.IsNullOrEmpty(configuration) && !string.IsNullOrEmpty(document))
			{


                var schema = GetDocumentSchema(version, configuration, document);
				if (schema != null)
				{
					var metadataIterator = new SchemaIterator(new SchemaReaderManager());

					metadataIterator.OnPrimitiveProperty = propertiesResult.Add;
					metadataIterator.OnObjectProperty = propertiesResult.Add;
					metadataIterator.OnArrayProperty = propertiesResult.Add;

					metadataIterator.ProcessSchema(version, schema);

				}
			}
			return propertiesResult;
		}


	}
}
