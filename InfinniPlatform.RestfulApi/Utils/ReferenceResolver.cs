using System.Collections.Generic;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.RestfulApi.Utils
{
    public sealed class ReferenceResolver : IReferenceResolver
    {
        private readonly IMetadataComponent _metadataComponent;

        public ReferenceResolver(IMetadataComponent metadataComponent)
        {
            _metadataComponent = metadataComponent;
        }

        public void ResolveReferences(string version, string configId, string documentId, dynamic documents,
                                      IEnumerable<dynamic> ignoreResolve)
        {
            var linkMap = new DocumentLinkMap(_metadataComponent);

            var metadataOperator = new MetadataOperator(_metadataComponent, linkMap, ignoreResolve);

            dynamic typeInfo = new DynamicWrapper();
            typeInfo.ConfigId = configId;
            typeInfo.DocumentId = documentId;


            if (documents is IEnumerable<dynamic>)
            {
                foreach (dynamic doc in documents)
                {
                    metadataOperator.ProcessMetadata(version, doc, typeInfo);
                }
            }
            else if (documents != null)
            {
                metadataOperator.ProcessMetadata(version, documents, typeInfo);
            }
            linkMap.ResolveLinks(version, typeInfo, metadataOperator.TypeInfoChain);
        }
    }
}