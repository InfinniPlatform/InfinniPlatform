using System.Collections.Generic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.SchemaReaders;

namespace InfinniPlatform.Api.Schema
{
    /// <summary>
    ///     Заполнитель схемы метаданных документа
    /// </summary>
    public sealed class PropertiesFiller
    {
        private dynamic GetDocumentSchema(string configuration, string document)
        {
            if (!string.IsNullOrEmpty(configuration) && !string.IsNullOrEmpty(document))
            {
                return new SchemaReaderManager().GetSchema(configuration, document);
            }
            return null;
        }

        public IEnumerable<SchemaObject> FillProperties(string configuration, string document,
            string alias, PathResolveType pathResolveType)
        {
            var propertiesResult = new List<SchemaObject>();
            if (!string.IsNullOrEmpty(configuration) && !string.IsNullOrEmpty(document))
            {
                var schema = GetDocumentSchema(configuration, document);
                if (schema != null)
                {
                    var metadataIterator = new SchemaIterator(new SchemaReaderManager());

                    metadataIterator.OnPrimitiveProperty = propertiesResult.Add;
                    metadataIterator.OnObjectProperty = propertiesResult.Add;
                    metadataIterator.OnArrayProperty = propertiesResult.Add;

                    metadataIterator.ProcessSchema(schema);
                }
            }
            return propertiesResult;
        }
    }
}