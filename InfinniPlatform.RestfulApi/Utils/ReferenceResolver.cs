using System.Collections.Generic;

using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.Utils
{
    public sealed class ReferenceResolver : IReferenceResolver
    {
        public ReferenceResolver(IMetadataComponent metadataComponent, DocumentLinkMapProvider documentLinkMapProvider)
        {
            _metadataComponent = metadataComponent;
            _documentLinkMapProvider = documentLinkMapProvider;
        }

        private readonly IMetadataComponent _metadataComponent;
        private readonly DocumentLinkMapProvider _documentLinkMapProvider;

        public void ResolveReferences(string configId, string documentId, dynamic documents, IEnumerable<dynamic> ignoreResolve)
        {
            var documentLinkMap = _documentLinkMapProvider.GetDocumentLinkMap();

            var metadataOperator = new MetadataOperator(_metadataComponent, documentLinkMap, ignoreResolve);

            dynamic typeInfo = new DynamicWrapper();
            typeInfo.ConfigId = configId;
            typeInfo.DocumentId = documentId;

            if (documents is IEnumerable<dynamic>)
            {
                foreach (var doc in documents)
                {
                    metadataOperator.ProcessMetadata(doc, typeInfo);
                }
            }
            else if (documents != null)
            {
                metadataOperator.ProcessMetadata(documents, typeInfo);
            }

            documentLinkMap.ResolveLinks(typeInfo, metadataOperator.TypeInfoChain);
        }
    }
}