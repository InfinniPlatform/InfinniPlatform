using System.Collections.Generic;

using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.Utils
{
    public sealed class ReferenceResolver : IReferenceResolver
    {
        public ReferenceResolver(IMetadataComponent metadataComponent, DocumentLinkMap documentLinkMap)
        {
            _metadataComponent = metadataComponent;
            _documentLinkMap = documentLinkMap;
        }

        private readonly DocumentLinkMap _documentLinkMap;
        private readonly IMetadataComponent _metadataComponent;

        public void ResolveReferences(string version, string configId, string documentId, dynamic documents, IEnumerable<dynamic> ignoreResolve)
        {
            var metadataOperator = new MetadataOperator(_metadataComponent, _documentLinkMap, ignoreResolve);

            dynamic typeInfo = new DynamicWrapper();
            typeInfo.ConfigId = configId;
            typeInfo.DocumentId = documentId;

            if (documents is IEnumerable<dynamic>)
            {
                foreach (var doc in documents)
                {
                    metadataOperator.ProcessMetadata(version, doc, typeInfo);
                }
            }
            else if (documents != null)
            {
                metadataOperator.ProcessMetadata(version, documents, typeInfo);
            }

            _documentLinkMap.ResolveLinks(version, typeInfo, metadataOperator.TypeInfoChain);
        }
    }
}