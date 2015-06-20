using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.RestfulApi.Utils
{
    public sealed class DocumentLinkMap
    {
        private readonly IList<DocumentLink> _links = new List<DocumentLink>();
        private readonly IMetadataComponent _metadataComponent;

        public DocumentLinkMap(IMetadataComponent metadataComponent)
        {
            _metadataComponent = metadataComponent;
        }


        public void RegisterLink(string configId, string documentId, string instanceId, Action<object> valueSetFunc)
        {
            var link = new DocumentLink();
            link.ConfigId = configId;
            link.DocumentId = documentId;
            link.InstanceId = instanceId;
            link.SetValue = valueSetFunc;

            _links.Add(link);
        }

        public void ResolveLinks(string version, dynamic resolvingTypeInfo, IEnumerable<dynamic> typeInfoChain)
        {
            var groupsLinks = _links.GroupBy(l => new
                {
                    l.ConfigId,
                    l.DocumentId
                }).ToList();


            foreach (var groupsLink in groupsLinks)
            {
                IEnumerable<DocumentLink> values = groupsLink.ToList();

                if (typeInfoChain != null &&
                    typeInfoChain.Count(
                        t => t.ConfigId == groupsLink.Key.ConfigId && t.DocumentId == groupsLink.Key.DocumentId) > 1)
                {
                    continue;
                }

                if (HasCircularRefs(version, groupsLink.Key.ConfigId, groupsLink.Key.DocumentId))
                {
                    continue;
                }

                List<dynamic> typeInfoChainUpdated = typeInfoChain.ToList();
                dynamic typeInfo = new DynamicWrapper();
                typeInfo.ConfigId = groupsLink.Key.ConfigId;
                typeInfo.DocumentId = groupsLink.Key.DocumentId;
                typeInfoChainUpdated.Add(typeInfo);

                Action<FilterBuilder> builder =
                    f => f.AddCriteria(c => c.Property("Id").IsIdIn(values.Select(i => i.InstanceId).ToList()));

                IEnumerable<dynamic> resolvedLinks =
                    new DocumentApiUnsecured(version).GetDocument(groupsLink.Key.ConfigId, groupsLink.Key.DocumentId,
                                                                  builder, 0, 100000, typeInfoChainUpdated).ToList();

                foreach (dynamic resolvedLink in resolvedLinks)
                {
                    IEnumerable<DocumentLink> doc = _links.Where(l => l.InstanceId == resolvedLink.Id);
                    foreach (DocumentLink documentLink in doc)
                    {
                        documentLink.SetValue(resolvedLink);
                    }
                }
            }
        }

        private bool HasCircularRefs(string version, string configId, string documentId)
        {
            IEnumerable<dynamic> metadata = _metadataComponent.GetMetadataList(version, configId, documentId,
                                                                               MetadataType.Schema);

            dynamic schema = metadata.FirstOrDefault();
            if (schema != null)
            {
                dynamic properties = schema.Properties;

                foreach (dynamic property in properties)
                {
                    if ((property.Value.Type != null &&
                         property.Value.Type.ToLowerInvariant() == "object" &&
                         property.Value.TypeInfo != null &&
                         property.Value.TypeInfo.DocumentLink != null &&
                         property.Value.TypeInfo.DocumentLink.ConfigId == configId &&
                         property.Value.TypeInfo.DocumentLink.DocumentId == documentId &&
                         property.Value.TypeInfo.DocumentLink.Resolve == true)
                        || (property.Value.Type != null &&
                            property.Value.Type.ToLowerInvariant() == "array" &&
                            property.Value.Items != null &&
                            property.Value.Items.TypeInfo != null &&
                            property.Value.Items.TypeInfo.DocumentLink != null &&
                            property.Value.Items.TypeInfo.DocumentLink.ConfigId == configId &&
                            property.Value.Items.TypeInfo.DocumentLink.DocumentId == documentId &&
                            property.Value.Items.TypeInfo.DocumentLink.Resolve == true)
                        || (property.Value.Items != null && property.Value.Items.TypeInfo != null &&
                            property.Value.Items.TypeInfo.DocumentLink != null &&
                            property.Value.Items.TypeInfo.DocumentLink.ConfigId == configId &&
                            property.Value.Items.TypeInfo.DocumentLink.DocumentId == documentId &&
                            property.Value.Items.TypeInfo.DocumentLink.Resolve == true)
                        || HasCircularArrayProperty(property, configId, documentId))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool HasCircularArrayProperty(dynamic property, string configId, string documentId)
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

            foreach (dynamic innerProperty in properties)
            {
                if (innerProperty.Value.TypeInfo != null && innerProperty.Value.TypeInfo.DocumentLink != null &&
                    innerProperty.Value.TypeInfo.DocumentLink.ConfigId == configId &&
                    innerProperty.Value.TypeInfo.DocumentLink.DocumentId == documentId &&
                    innerProperty.Value.TypeInfo.DocumentLink.Resolve == true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}