using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.ContextComponents;

namespace InfinniPlatform.RestfulApi.Utils
{
	public sealed class DocumentExecutor
	{
		private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
		private readonly IMetadataComponent _metadataComponent;
		private readonly InprocessDocumentComponent _documentComponent;
		private readonly IProfilerComponent _profilerComponent;

		public DocumentExecutor(IConfigurationMediatorComponent configurationMediatorComponent, IMetadataComponent metadataComponent, InprocessDocumentComponent documentComponent, IProfilerComponent profilerComponent)
		{
			_configurationMediatorComponent = configurationMediatorComponent;
			_metadataComponent = metadataComponent;
			_documentComponent = documentComponent;
			_profilerComponent = profilerComponent;
		}

		public IEnumerable<dynamic> GetCompleteDocuments(string configId, string documentId, string userName, int pageNumber, int pageSize,
			IEnumerable<dynamic> filter, IEnumerable<dynamic> sorting, IEnumerable<dynamic> ignoreResolve)
		{
			var documentProvider = _documentComponent.GetDocumentProvider(configId, documentId, userName);

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


				var schema = metadataConfiguration.GetSchemaVersion(documentId);

				IEnumerable<dynamic> sortingFields = null;

				if (schema != null)
				{
					// Ивлекаем информацию о полях, по которым можно проводить сортировку из метаданных документа
					sortingFields = ExtractSortingProperties("", schema.Properties, _configurationMediatorComponent.ConfigurationBuilder);
				}

				if (sorting != null && sorting.Any())
				{
					// Поля сортировки заданы в запросе. 
					// Берем только те поля, которые разрешены в соответствии с метаданными

					var filteredSortingFields = new List<object>();

					foreach (var sortingProperty in sorting.ToEnumerable())
					{
						if (sortingFields.ToEnumerable().Any(validProperty => validProperty.PropertyName == sortingProperty.PropertyName))
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


				var profiler = _profilerComponent.GetOperationProfiler("VersionProvider.GetDocument", null);
				profiler.Reset();

				//делаем выборку документов для последующего Resolve и фильтрации по полям Resolved объектов
				var pageSizeUnresolvedDocuments = Math.Max(pageSize, AppSettings.GetValue("ResolvedRecordNumber", 1000));

				var criteriaInterpreter = new QueryCriteriaInterpreter();

				var queryAnalyzer = new QueryCriteriaAnalyzer(_metadataComponent, schema);

				IEnumerable<dynamic> result = documentProvider.GetDocument(queryAnalyzer.GetBeforeResolveCriteriaList(filter), 0, 
					Convert.ToInt32(pageSizeUnresolvedDocuments), sorting, pageNumber > 0 ? pageNumber * pageSize : 0 );

				new ReferenceResolver(_metadataComponent).ResolveReferences(configId, documentId, result, ignoreResolve);

				result = criteriaInterpreter.ApplyFilter(queryAnalyzer.GetAfterResolveCriteriaList(filter), result);

				profiler.TakeSnapshot();

				return result.Take(pageSize == 0 ? 10 : pageSize);
			}
			return new List<dynamic>();
		}

		private static IEnumerable<object> ExtractSortingProperties(string rootName, dynamic properties, IConfigurationObjectBuilder configurationObjectBuilder)
		{
			var sortingProperties = new List<object>();

			if (properties != null)
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
							//var inlineMetadataConfiguration = metadataConfigurationProvider.Configurations.FirstOrDefault(
							//	c => c.ConfigurationId == propertyMapping.Value.TypeInfo.DocumentLink.ConfigId);

							var inlineMetadataConfiguration =
								configurationObjectBuilder.GetConfigurationObject(propertyMapping.Value.TypeInfo.DocumentLink.ConfigId)
														  .MetadataConfiguration;

							if (inlineMetadataConfiguration != null)
							{
								var inlineDocumentSchema = inlineMetadataConfiguration.GetSchemaVersion(propertyMapping.Value.TypeInfo.DocumentLink.DocumentId);

								if (inlineDocumentSchema != null)
								{
									childProps = ExtractSortingProperties(formattedPropertyName, inlineDocumentSchema.Properties, configurationObjectBuilder);
								}
							}
						}
						else
						{
							childProps = ExtractSortingProperties(formattedPropertyName, propertyMapping.Value.Properties, configurationObjectBuilder);
						}

						sortingProperties.AddRange(childProps);

					}
					else if (propertyMapping.Value.Type.ToString() == DataType.Array.ToString())
					{
						if (propertyMapping.Value.Items != null)
						{
							sortingProperties.AddRange(
								ExtractSortingProperties(formattedPropertyName, propertyMapping.Value.Items.Properties, configurationObjectBuilder));
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

			return sortingProperties.ToArray();
		}

	}
}
