using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.RestfulApi.Utils
{
    public sealed class DocumentExecutor
    {
        public DocumentExecutor(IConfigurationMediatorComponent configurationMediatorComponent,
                                IMetadataComponent metadataComponent,
                                InprocessDocumentComponent documentComponent,
                                IReferenceResolver referenceResolver)
        {
            _configurationMediatorComponent = configurationMediatorComponent;
            _metadataComponent = metadataComponent;
            _documentComponent = documentComponent;
            _referenceResolver = referenceResolver;
        }

        private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
        private readonly InprocessDocumentComponent _documentComponent;
        private readonly IMetadataComponent _metadataComponent;
        private readonly IReferenceResolver _referenceResolver;

        public dynamic GetBaseDocument(string instanceId)
        {
            var documentProvider = _documentComponent.GetAllIndexesOperationProvider();
            return documentProvider.GetItem(instanceId);
        }

        public dynamic GetCompleteDocument(string configId, string documentId, string instanceId)
        {
            var docsToResolve = new[] { GetBaseDocument(instanceId) };
            _referenceResolver.ResolveReferences(configId, documentId, docsToResolve, null);
            return docsToResolve.FirstOrDefault();
        }

        public IEnumerable<dynamic> GetCompleteDocuments(string configId, string documentId,
                                                         int pageNumber, int pageSize,
                                                         IEnumerable<dynamic> filter, IEnumerable<dynamic> sorting,
                                                         IEnumerable<dynamic> ignoreResolve)
        {
            var documentProvider = _documentComponent.GetDocumentProvider(configId, documentId);

            if (documentProvider != null)
            {
                var metadataConfiguration =
                    _configurationMediatorComponent
                        .ConfigurationBuilder.GetConfigurationObject(configId)
                        .MetadataConfiguration;

                if (metadataConfiguration == null)
                {
                    return new List<dynamic>();
                }


                dynamic schema = metadataConfiguration.GetSchemaVersion(documentId);

                IEnumerable<dynamic> sortingFields = null;

                if (schema != null)
                {
                    // Ивлекаем информацию о полях, по которым можно проводить сортировку из метаданных документа
                    sortingFields = ExtractSortingProperties("", schema.Properties,
                        _configurationMediatorComponent.ConfigurationBuilder);
                }

                if (sorting != null && sorting.Any())
                {
                    // Поля сортировки заданы в запросе. 
                    // Берем только те поля, которые разрешены в соответствии с метаданными

                    var filteredSortingFields = new List<object>();

                    foreach (var sortingProperty in sorting.ToEnumerable())
                    {
                        if (
                            sortingFields.ToEnumerable()
                                         .Any(
                                             validProperty => validProperty.PropertyName == sortingProperty.PropertyName))
                        {
                            filteredSortingFields.Add(sortingProperty);
                        }
                    }

                    sorting = filteredSortingFields;
                }
                else if (sortingFields != null && sortingFields.Any())
                {
                    sorting = sortingFields;
                }


                //делаем выборку документов для последующего Resolve и фильтрации по полям Resolved объектов
                var pageSizeUnresolvedDocuments = Math.Min(pageSize, 1000);

                var criteriaInterpreter = new QueryCriteriaInterpreter();

                var queryAnalyzer = new QueryCriteriaAnalyzer(_metadataComponent, schema);

                IEnumerable<dynamic> result =
                    documentProvider.GetDocument(queryAnalyzer.GetBeforeResolveCriteriaList(filter), 0,
                        Convert.ToInt32(pageSizeUnresolvedDocuments), sorting,
                        pageNumber > 0 ? pageNumber * pageSize : 0);

                _referenceResolver.ResolveReferences(configId, documentId, result, ignoreResolve);

                result = criteriaInterpreter.ApplyFilter(queryAnalyzer.GetAfterResolveCriteriaList(filter), result);

                return result.Take(pageSize == 0 ? 10 : pageSize);
            }
            return new List<dynamic>();
        }

        private static IEnumerable<object> ExtractSortingProperties(string rootName, dynamic properties, IConfigurationObjectBuilder configurationObjectBuilder)
        {
            var sortingProperties = new List<object>();

            if (properties != null)
            {
                foreach (var propertyMapping in properties)
                {
                    string formattedPropertyName = string.IsNullOrEmpty(rootName)
                        ? string.Format("{0}", propertyMapping.Key)
                        : string.Format("{0}.{1}", rootName, propertyMapping.Key);

                    if (propertyMapping.Value.Type.ToString() == DataType.Object.ToString())
                    {
                        var childProps = new object[] { };

                        if (propertyMapping.Value.TypeInfo != null &&
                            propertyMapping.Value.TypeInfo.DocumentLink != null &&
                            propertyMapping.Value.TypeInfo.DocumentLink.Inline != null &&
                            propertyMapping.Value.TypeInfo.DocumentLink.Inline == true)
                        {
                            // inline ссылка на документ: необходимо получить схему документа, на который сделана ссылка,
                            // чтобы получить сортировочные поля 
                            dynamic inlineMetadataConfiguration =
                                configurationObjectBuilder.GetConfigurationObject(propertyMapping.Value.TypeInfo
                                                                                                 .DocumentLink.ConfigId)
                                                          .MetadataConfiguration;

                            if (inlineMetadataConfiguration != null)
                            {
                                dynamic inlineDocumentSchema =
                                    inlineMetadataConfiguration.GetSchemaVersion(
                                        propertyMapping.Value.TypeInfo.DocumentLink.DocumentId);

                                if (inlineDocumentSchema != null)
                                {
                                    childProps = ExtractSortingProperties(formattedPropertyName,
                                        inlineDocumentSchema.Properties,
                                        configurationObjectBuilder);
                                }
                            }
                        }
                        else
                        {
                            childProps = ExtractSortingProperties(formattedPropertyName,
                                propertyMapping.Value.Properties,
                                configurationObjectBuilder);
                        }

                        sortingProperties.AddRange(childProps);
                    }
                    else if (propertyMapping.Value.Type.ToString() == DataType.Array.ToString())
                    {
                        if (propertyMapping.Value.Items != null)
                        {
                            sortingProperties.AddRange(
                                ExtractSortingProperties(formattedPropertyName,
                                    propertyMapping.Value.Items.Properties,
                                    configurationObjectBuilder));
                        }
                    }
                    else
                    {
                        var isSortingField = false;

                        if (propertyMapping.Value.Sortable != null)
                        {
                            isSortingField = propertyMapping.Value.Sortable;
                        }

                        if (isSortingField)
                        {
                            sortingProperties.Add(new
                                                  {
                                                      PropertyName = formattedPropertyName,
                                                      SortOrder = SortOrder.Ascending
                                                  }.ToDynamic());
                        }
                    }
                }
            }

            return sortingProperties.ToArray();
        }
    }
}