using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Utils
{
    public sealed class ReferenceResolver : IReferenceResolver
    {
        public ReferenceResolver(IMetadataApi metadataApi, DocumentLinkMapProvider documentLinkMapProvider)
        {
            _metadataApi = metadataApi;
            _documentLinkMapProvider = documentLinkMapProvider;
        }

        private readonly IMetadataApi _metadataApi;
        private readonly DocumentLinkMapProvider _documentLinkMapProvider;

        public void ResolveReferences(string configId, string documentId, dynamic documents, IEnumerable<dynamic> ignoreResolve)
        {
            var documentLinkMap = _documentLinkMapProvider.GetDocumentLinkMap();

            var metadataOperator = new MetadataOperator(_metadataApi, documentLinkMap, ignoreResolve);

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