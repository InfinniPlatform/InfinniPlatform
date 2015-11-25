using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Profiling;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Profiling;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.RestfulApi.Utils
{
    public sealed class DocumentExecutor
    {
        private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
        private readonly InprocessDocumentComponent _documentComponent;
        private readonly IMetadataComponent _metadataComponent;
        private readonly IProfilerComponent _profilerComponent;
        private readonly ILogComponent _logComponent;

        public DocumentExecutor(IConfigurationMediatorComponent configurationMediatorComponent,
                                IMetadataComponent metadataComponent, InprocessDocumentComponent documentComponent,
                                IProfilerComponent profilerComponent, ILogComponent logComponent)
        {
            _configurationMediatorComponent = configurationMediatorComponent;
            _metadataComponent = metadataComponent;
            _documentComponent = documentComponent;
            _profilerComponent = profilerComponent;
            _logComponent = logComponent;
        }

        public dynamic GetBaseDocument(string userName, string instanceId)
        {
            IAllIndexesOperationProvider documentProvider = _documentComponent.GetAllIndexesOperationProvider();
            return documentProvider.GetItem(instanceId);
        }

        public dynamic GetCompleteDocument(string version, string configId, string documentId, string userName,
                                           string instanceId)
        {
            var docsToResolve = new[] {GetBaseDocument(userName, instanceId)};
            new ReferenceResolver(_metadataComponent).ResolveReferences(version, configId, documentId,
                                                                        docsToResolve, null);
            return docsToResolve.FirstOrDefault();
        }

        public IEnumerable<dynamic> GetCompleteDocuments(string version, string configId, string documentId,
                                                         string userName, int pageNumber, int pageSize,
                                                         IEnumerable<dynamic> filter, IEnumerable<dynamic> sorting,
                                                         IEnumerable<dynamic> ignoreResolve)
        {

            IVersionProvider documentProvider = _documentComponent.GetDocumentProvider(configId, documentId);

            if (documentProvider != null)
            {

                IMetadataConfiguration metadataConfiguration =
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
                    sortingFields = ExtractSortingProperties(version, "", schema.Properties,
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


                IOperationProfiler profiler = _profilerComponent.GetOperationProfiler("VersionProvider.GetDocument",
                                                                                      null);
                profiler.Reset();

                //делаем выборку документов для последующего Resolve и фильтрации по полям Resolved объектов
                int pageSizeUnresolvedDocuments = Math.Max(pageSize, AppSettings.GetValue("ResolvedRecordNumber", 1000));

                var criteriaInterpreter = new QueryCriteriaInterpreter();

                var queryAnalyzer = new QueryCriteriaAnalyzer(_metadataComponent, version, schema);

                IEnumerable<dynamic> result =
                    documentProvider.GetDocument(queryAnalyzer.GetBeforeResolveCriteriaList(filter), 0,
                                                 Convert.ToInt32(pageSizeUnresolvedDocuments), sorting,
                                                 pageNumber > 0 ? pageNumber*pageSize : 0);

                new ReferenceResolver(_metadataComponent).ResolveReferences(version, configId, documentId, result,
                                                                            ignoreResolve);

                result = criteriaInterpreter.ApplyFilter(queryAnalyzer.GetAfterResolveCriteriaList(filter), result);

                profiler.TakeSnapshot();

                return result.Take(pageSize == 0 ? 10 : pageSize);
            }
            return new List<dynamic>();
        }

        private static IEnumerable<object> ExtractSortingProperties(string version, string rootName, dynamic properties,
                                                                    IConfigurationObjectBuilder
                                                                        configurationObjectBuilder)
        {
            var sortingProperties = new List<object>();

            if (properties != null)
                foreach (dynamic propertyMapping in properties)
                {
                    string formattedPropertyName = string.IsNullOrEmpty(rootName)
                                                       ? string.Format("{0}", propertyMapping.Key)
                                                       : string.Format("{0}.{1}", rootName, propertyMapping.Key);

                    if (propertyMapping.Value.Type.ToString() == DataType.Object.ToString())
                    {
                        var childProps = new object[] {};

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
                                    childProps = ExtractSortingProperties(version, formattedPropertyName,
                                                                          inlineDocumentSchema.Properties,
                                                                          configurationObjectBuilder);
                                }
                            }
                        }
                        else
                        {
                            childProps = ExtractSortingProperties(version, formattedPropertyName,
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
                                ExtractSortingProperties(version, formattedPropertyName,
                                                         propertyMapping.Value.Items.Properties,
                                                         configurationObjectBuilder));
                        }
                    }
                    else
                    {
                        bool isSortingField = false;

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

            return sortingProperties.ToArray();
        }
    }
}