using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.RestApi.Auth;

namespace InfinniPlatform.Metadata.Implementation.Handlers
{
    public class SearchDocumentAggregationHandler
    {
        private readonly IGlobalContext _globalContext;
        private readonly IFilterBuilder _filterFactory = FilterBuilderFactory.GetInstance();

        public IConfigRequestProvider ConfigRequestProvider { get; set; }

        public SearchDocumentAggregationHandler(IGlobalContext globalContext)
        {
            _globalContext = globalContext;
        }

        public dynamic GetAggregationDocumentResult(
            string aggregationConfiguration,
            string aggregationMetadata, 
            IEnumerable<dynamic> filterObject, 
            IEnumerable<dynamic> dimensions,
            IEnumerable<AggregationType> aggregationTypes,
            IEnumerable<string> aggregationFields, 
            int pageNumber, 
            int pageSize)
        {
            //получаем тип метаданных/индекс, по которому выполняем поиск документов
            var idType = ConfigRequestProvider.GetMetadataIdentifier();

			var metadataConfig = _globalContext.GetComponent<IMetadataConfigurationProvider>().GetMetadataConfiguration(ConfigRequestProvider.GetConfiguration());

            var target = new SearchContext();
            target.Context = _globalContext;
            target.Index = metadataConfig.ConfigurationId;
            target.IndexType = metadataConfig.GetMetadataIndexType(idType);
            target.IsValid = true;
            target.Configuration = ConfigRequestProvider.GetConfiguration();
            target.Metadata = ConfigRequestProvider.GetMetadataIdentifier();

            metadataConfig.MoveWorkflow(idType, metadataConfig.GetExtensionPointValue(ConfigRequestProvider, "Join"), target);

			//в качестве routing используется клэйм организации пользователя
			var executor = target.Context.GetComponent<IIndexComponent>().IndexFactory.BuildAggregationProvider(aggregationConfiguration, aggregationMetadata, 
				target.Context.GetComponent<ISecurityComponent>().GetClaim(AuthorizationStorageExtensions.OrganizationClaim, target.UserName) ?? AuthorizationStorageExtensions.AnonimousUser);
            
            //заполняем предварительные результаты поиска
            target.SearchResult = executor.ExecuteAggregation(
                dimensions.ToArray(), 
                aggregationTypes.ToArray(),
                aggregationFields.ToArray(), 
                filterObject.ExtractSearchModel(_filterFactory).Filter); 
            
            //выполняем постобработку результатов
            metadataConfig.MoveWorkflow(idType, metadataConfig.GetExtensionPointValue(ConfigRequestProvider, "TransformResult"), target);

            return target.SearchResult;
        }
    }
}