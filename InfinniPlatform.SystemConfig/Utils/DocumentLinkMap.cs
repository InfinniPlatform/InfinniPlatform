using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.RestApi.CommonApi;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.IoC;

using ApplyChangesHandler = InfinniPlatform.SystemConfig.RequestHandlers.ApplyChangesHandler;

namespace InfinniPlatform.SystemConfig.Utils
{
    public sealed class DocumentLinkMap
    {
        public DocumentLinkMap(IMetadataComponent metadataComponent, RestQueryApi restQueryApi, Func<ApplyChangesHandler> applyChangesHandlerFactory)
        {
            _metadataComponent = metadataComponent;
            _restQueryApi = restQueryApi;
            _applyChangesHandlerFactory = applyChangesHandlerFactory;
        }

        private readonly RestQueryApi _restQueryApi;
        private readonly Func<ApplyChangesHandler> _applyChangesHandlerFactory;
        private readonly IList<DocumentLink> _links = new List<DocumentLink>();
        private readonly IMetadataComponent _metadataComponent;

        public void RegisterLink(string configId, string documentId, string instanceId, Action<object> valueSetFunc)
        {
            var link = new DocumentLink
            {
                ConfigId = configId,
                DocumentId = documentId,
                InstanceId = instanceId,
                SetValue = valueSetFunc
            };

            lock (_links)
            {
                _links.Add(link);
            }
        }

        public void ResolveLinks(dynamic resolvingTypeInfo, IEnumerable<dynamic> typeInfoChain)
        {
            IEnumerable<DocumentLink> links;

            lock (_links)
            {
                links = _links.ToArray();
            }

            var groupsLinks = links.GroupBy(l => new
                                                 {
                                                     l.ConfigId,
                                                     l.DocumentId
                                                 })
                                   .ToList();

            foreach (var groupsLink in groupsLinks)
            {
                IEnumerable<DocumentLink> values = groupsLink.ToList();

                if (typeInfoChain != null &&
                    typeInfoChain.Count(t => t.ConfigId == groupsLink.Key.ConfigId && t.DocumentId == groupsLink.Key.DocumentId) > 1)
                {
                    continue;
                }

                if (HasCircularRefs(groupsLink.Key.ConfigId, groupsLink.Key.DocumentId))
                {
                    continue;
                }

                var typeInfoChainUpdated = typeInfoChain.ToList();
                dynamic typeInfo = new DynamicWrapper();
                typeInfo.ConfigId = groupsLink.Key.ConfigId;
                typeInfo.DocumentId = groupsLink.Key.DocumentId;
                typeInfoChainUpdated.Add(typeInfo);

                Action<FilterBuilder> builder = f => f.AddCriteria(c => c.Property("Id").IsIdIn(values.Select(i => i.InstanceId).ToList()));

                var resolvedLinks = GetResolvedLinks(groupsLink, builder, typeInfoChainUpdated);

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

        private List<dynamic> GetResolvedLinks(IGrouping<dynamic, DocumentLink> groupsLink, Action<FilterBuilder> filter, List<dynamic> typeInfoChainUpdated)
        {
            var filterBuilder = new FilterBuilder();

            filter?.Invoke(filterBuilder);

            dynamic request = new DynamicWrapper();
            request.Configuration = groupsLink.Key.ConfigId;
            request.Metadata = groupsLink.Key.DocumentId;
            request.Filter = filterBuilder.CriteriaList;
            request.PageNumber = 0;
            request.PageSize = 100;
            request.IgnoreResolve = typeInfoChainUpdated;

            var applyChangesHandler = _applyChangesHandlerFactory();
            applyChangesHandler.ConfigRequestProvider = new LocalDataProvider("SystemConfig", "configuration", "getdocument");

            var result = applyChangesHandler.ApplyJsonObject(null, request);

            var resolvedLinks = (result != null) ? Enumerable.ToList(result) : null;

            return resolvedLinks;
        }

        private bool HasCircularRefs(string configId, string documentId)
        {
            var metadata = _metadataComponent.GetMetadataList(configId, documentId, "Schema");

            dynamic schema = metadata.FirstOrDefault();
            if (schema != null)
            {
                dynamic properties = schema.Properties;

                foreach (var property in properties)
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

            foreach (var innerProperty in properties)
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

    public class DocumentLinkMapProvider
    {
        public DocumentLinkMapProvider(Func<IContainerResolver> containerFactory)
        {
            _documentLinkMapFactory = () => containerFactory().Resolve<DocumentLinkMap>();
        }

        private readonly Func<DocumentLinkMap> _documentLinkMapFactory;

        public DocumentLinkMap GetDocumentLinkMap()
        {
            return _documentLinkMapFactory();
        }
    }
}