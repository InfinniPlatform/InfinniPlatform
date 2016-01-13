using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.ContextTypes;
using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.ElasticSearch.Filters;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    public sealed class SearchDocumentAggregationHandler : IWebRoutingHandler
    {
        public SearchDocumentAggregationHandler(IMetadataConfigurationProvider metadataConfigurationProvider, IIndexFactory indexFactory)
        {
            _metadataConfigurationProvider = metadataConfigurationProvider;
            _indexFactory = indexFactory;
            _filterFactory = FilterBuilderFactory.GetInstance();
        }


        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;
        private readonly IIndexFactory _indexFactory;
        private readonly INestFilterBuilder _filterFactory;


        public IConfigRequestProvider ConfigRequestProvider { get; set; }


        public dynamic GetAggregationDocumentResult(
            string aggregationConfiguration,
            string aggregationMetadata,
            IEnumerable<FilterCriteria> filterCriteria,
            IEnumerable<dynamic> dimensions,
            IEnumerable<AggregationType> aggregationTypes,
            IEnumerable<string> aggregationFields,
            int pageNumber,
            int pageSize)
        {
            var сonfiguration = ConfigRequestProvider.GetConfiguration();
            var documentType = ConfigRequestProvider.GetMetadataIdentifier();

            // Метаданные конфигурации
            var metadataConfiguration = _metadataConfigurationProvider.GetMetadataConfiguration(сonfiguration);

            var target = new SearchContext
            {
                Index = metadataConfiguration.ConfigurationId,
                IndexType = metadataConfiguration.GetMetadataIndexType(documentType),
                Configuration = сonfiguration,
                Metadata = documentType,
                IsValid = true
            };

            ExecuteExtensionPoint(metadataConfiguration, documentType, "Join", target);

            var executor = _indexFactory.BuildAggregationProvider(aggregationConfiguration, aggregationMetadata);

            // Заполняем предварительные результаты поиска
            target.SearchResult = executor.ExecuteAggregation(
                dimensions.ToArray(),
                aggregationTypes.ToArray(),
                aggregationFields.ToArray(),
                filterCriteria.ExtractSearchModel(_filterFactory));

            ExecuteExtensionPoint(metadataConfiguration, documentType, "TransformResult", target);

            return target.SearchResult;
        }

        private bool ExecuteExtensionPoint(IMetadataConfiguration metadataConfiguration, string documentType, string extensionPointName, ICommonContext extensionPointContext)
        {
            var extensionPoint = metadataConfiguration.GetExtensionPointValue(ConfigRequestProvider, extensionPointName);

            if (!string.IsNullOrEmpty(extensionPoint))
            {
                metadataConfiguration.MoveWorkflow(documentType, extensionPoint, extensionPointContext);
            }

            return extensionPointContext.IsValid;
        }
    }
}