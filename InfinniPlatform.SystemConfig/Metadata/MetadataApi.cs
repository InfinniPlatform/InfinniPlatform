using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Metadata
{
    internal sealed class MetadataApi : IMetadataApi
    {
        private readonly Dictionary<string, DynamicWrapper> _itemsMetadata = new Dictionary<string, DynamicWrapper>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<string> GetNames(string startsWithMask)
        {
            return _itemsMetadata.Keys
                                 .Where(i => i.StartsWith(startsWithMask))
                                 .Select(i => i.Replace(startsWithMask, string.Empty));
        }

        public dynamic GetMetadata(string metadataName)
        {
            DynamicWrapper metadata;

            return !string.IsNullOrEmpty(metadataName)
                   && _itemsMetadata.TryGetValue(metadataName, out metadata)
                       ? metadata
                       : null;
        }

        public IEnumerable<string> GetDocumentNames()
        {
            return GetNames("Documents.");
        }

        public dynamic GetDocumentSchema(string documentName)
        {
            documentName = $"Documents.{documentName}";

            DynamicWrapper document;

            return !string.IsNullOrEmpty(documentName)
                   && _itemsMetadata.TryGetValue(documentName, out document)
                       ? document["Schema"]
                       : null;
        }

        public dynamic GetDocumentEvents(string documentName)
        {
            documentName = $"Documents.{documentName}";

            DynamicWrapper document;

            return !string.IsNullOrEmpty(documentName)
                   && _itemsMetadata.TryGetValue(documentName, out document)
                       ? document["Events"]
                       : null;
        }

        public IEnumerable<object> GetDocumentIndexes(string documentName)
        {
            documentName = $"Documents.{documentName}";

            DynamicWrapper document;

            return !string.IsNullOrEmpty(documentName)
                   && _itemsMetadata.TryGetValue(documentName, out document)
                       ? document["Indexes"] as IEnumerable<object>
                       : null;
        }

        public void AddItemsMetadata(Dictionary<string, DynamicWrapper> itemsDictionary)
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