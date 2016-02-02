using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.SystemConfig.Utils
{
    public sealed class DocumentLinkMap
    {
        public delegate IEnumerable<dynamic> GetDocumentsDelegate(
            string documentType,
            IEnumerable<FilterCriteria> filter,
            int pageNumber,
            int pageSize,
            IEnumerable<SortingCriteria> sorting = null);


        public DocumentLinkMap(IMetadataApi metadataComponent, GetDocumentsDelegate getDocuments)
        {
            _metadataComponent = metadataComponent;
            _getDocuments = getDocuments;
        }

        private readonly IList<DocumentLink> _links = new List<DocumentLink>();
        private readonly IMetadataApi _metadataComponent;
        private readonly GetDocumentsDelegate _getDocuments;

        public void RegisterLink(string documentId, string instanceId, Action<object> valueSetFunc)
        {
            var link = new DocumentLink
            {
                DocumentId = documentId,
                InstanceId = instanceId,
                SetValue = valueSetFunc
            };

            lock (_links)
            {
                _links.Add(link);
            }
        }

        public void ResolveLinks(IEnumerable<dynamic> typeInfoChain)
        {
            IEnumerable<DocumentLink> links;

            lock (_links)
            {
                links = _links.ToArray();
            }

            var groupsLinks = links.GroupBy(l => l.DocumentId).ToList();

            foreach (var groupsLink in groupsLinks)
            {
                IEnumerable<DocumentLink> values = groupsLink.ToList();

                if (typeInfoChain != null && typeInfoChain.Count(t => t.DocumentId == groupsLink.Key) > 1)
                {
                    continue;
                }

                if (HasCircularRefs(groupsLink.Key))
                {
                    continue;
                }

                Action<FilterBuilder> builder = f => f.AddCriteria(c => c.Property("Id").IsIdIn(values.Select(i => i.InstanceId).ToList()));

                var resolvedLinks = GetResolvedLinks(groupsLink, builder);

                foreach (var resolvedLink in resolvedLinks)
                {
                    var doc = links.Where(l => l.InstanceId == resolvedLink.Id);
                    foreach (var documentLink in doc)
                    {
                        documentLink.SetValue(resolvedLink);
                    }
                }
            }
        }

        private List<dynamic> GetResolvedLinks(IGrouping<string, DocumentLink> groupsLink, Action<FilterBuilder> filterAction)
        {
            var documentType = groupsLink.Key;
            var filter = filterAction.ToFilterCriterias();

            IEnumerable<dynamic> result = _getDocuments(documentType, filter, 0, 100);

            var resolvedLinks = (result != null) ? Enumerable.ToList(result) : null;

            return resolvedLinks;
        }

        private bool HasCircularRefs(string documentId)
        {
            dynamic schema = _metadataComponent.GetDocumentSchema(documentId);

            if (schema != null)
            {
                dynamic properties = schema.Properties;

                foreach (var property in properties)
                {
                    if ((property.Value.Type != null &&
                         property.Value.Type.ToLowerInvariant() == "object" &&
                         property.Value.TypeInfo != null &&
                         property.Value.TypeInfo.DocumentLink != null &&
                         property.Value.TypeInfo.DocumentLink.DocumentId == documentId &&
                         property.Value.TypeInfo.DocumentLink.Resolve == true)
                        || (property.Value.Type != null &&
                            property.Value.Type.ToLowerInvariant() == "array" &&
                            property.Value.Items != null &&
                            property.Value.Items.TypeInfo != null &&
                            property.Value.Items.TypeInfo.DocumentLink != null &&
                            property.Value.Items.TypeInfo.DocumentLink.DocumentId == documentId &&
                            property.Value.Items.TypeInfo.DocumentLink.Resolve == true)
                        || (property.Value.Items != null && property.Value.Items.TypeInfo != null &&
                            property.Value.Items.TypeInfo.DocumentLink != null &&
                            property.Value.Items.TypeInfo.DocumentLink.DocumentId == documentId &&
                            property.Value.Items.TypeInfo.DocumentLink.Resolve == true)
                        || HasCircularArrayProperty(property, documentId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool HasCircularArrayProperty(dynamic property, string documentId)
        {
            if (property.Value.Items == null)
            {
                return false;
            }

            dynamic properties = property.Value.Items.Properties;

            if (properties == null)
            {
                return false;
            }

            foreach (var innerProperty in properties)
            {
                if (innerProperty.Value.TypeInfo != null && innerProperty.Value.TypeInfo.DocumentLink != null &&
                    innerProperty.Value.TypeInfo.DocumentLink.DocumentId == documentId &&
                    innerProperty.Value.TypeInfo.DocumentLink.Resolve == true)
                {
                    return true;
                }
            }
            return false;
        }
    }


    public sealed class DocumentLinkMapProvider
    {
        public DocumentLinkMapProvider(IContainerResolver containerResolver)
        {
            _documentLinkMapFactory = () =>
                                      {
                                          var documentApi = containerResolver.Resolve<IDocumentApi>();
                                          var metadataComponent = containerResolver.Resolve<IMetadataApi>();
                                          return new DocumentLinkMap(metadataComponent, (documentType, filter, pageNumber, pageSize, sorting) => documentApi.GetDocuments(documentType, filter, pageNumber, pageSize, sorting));
                                      };
        }

        private readonly Func<DocumentLinkMap> _documentLinkMapFactory;

        public DocumentLinkMap GetDocumentLinkMap()
        {
            return _documentLinkMapFactory();
        }
    }
}