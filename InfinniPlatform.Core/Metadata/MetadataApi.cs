using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Metadata
{
    internal sealed class MetadataApi : IMetadataApi
    {
        //TODO Избавиться от захардкоженых значений типа "Documents" после удаления deprecated кода работы с ElasticSearch.
        private readonly Dictionary<MetadataUniqueName, DynamicWrapper> _itemsMetadata = new Dictionary<MetadataUniqueName, DynamicWrapper>();

        public IEnumerable<string> GetMetadataItemNames(string partOfFullName)
        {
            return _itemsMetadata.Keys
                                 .Where(i => i.Namespace.Contains(partOfFullName))
                                 .Select(i => i.Name);
        }

        public dynamic GetMetadata(string metadataName)
        {
            DynamicWrapper metadata;

            return !string.IsNullOrEmpty(metadataName)
                   && _itemsMetadata.TryGetValue(new MetadataUniqueName(metadataName), out metadata)
                       ? metadata
                       : null;
        }

        public dynamic GetDocumentSchema(string documentName)
        {
            DynamicWrapper document;

            return !string.IsNullOrEmpty(documentName)
                   && _itemsMetadata.TryGetValue(new MetadataUniqueName("Documents", documentName), out document)
                       ? document["Schema"]
                       : null;
        }

        public dynamic GetDocumentEvents(string documentName)
        {
            DynamicWrapper document;

            return !string.IsNullOrEmpty(documentName)
                   && _itemsMetadata.TryGetValue(new MetadataUniqueName("Documents", documentName), out document)
                       ? document["Events"]
                       : null;
        }

        public IEnumerable<object> GetDocumentIndexes(string documentName)
        {
            DynamicWrapper document;

            return !string.IsNullOrEmpty(documentName)
                   && _itemsMetadata.TryGetValue(new MetadataUniqueName("Documents", documentName), out document)
                       ? document["Indexes"] as IEnumerable<object>
                       : null;
        }

        public void AddItemsMetadata(Dictionary<MetadataUniqueName, DynamicWrapper> itemsDictionary)
        {
            foreach (var item in itemsDictionary)
            {
                if (_itemsMetadata.ContainsKey(item.Key))
                {
                    _itemsMetadata[item.Key] = item.Value;
                }
                else
                {
                    _itemsMetadata.Add(item.Key, item.Value);
                }
            }
        }
    }
}