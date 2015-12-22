using System.Collections.Generic;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
	public sealed class ActionUnitGetNumberOfDocuments
	{
        private IConfigurationMediatorComponent _configurationMediatorComponent;
        private IMetadataComponent _metadataComponent;
        private InprocessDocumentComponent _documentComponent;
        private IProfilerComponent _profilerComponent;

		public void Action(IApplyContext target)
		{
            dynamic returnResult = new DynamicWrapper();
            returnResult.NumberOfDocuments = 0;
            returnResult.IsValid = true;

            _configurationMediatorComponent = target.Context.GetComponent<IConfigurationMediatorComponent>();
            _metadataComponent = target.Context.GetComponent<IMetadataComponent>();
            _documentComponent = target.Context.GetComponent<InprocessDocumentComponent>();
            _profilerComponent = target.Context.GetComponent<IProfilerComponent>();

            string configId = target.Item.Configuration;
            string documentId = target.Item.Metadata;
            string userName = target.UserName;
            IEnumerable<dynamic> filter = target.Item.Filter;

            var documentProvider = _documentComponent.GetDocumentProvider(configId, documentId);

            if (documentProvider != null)
            {
                var metadataConfiguration =
                    _configurationMediatorComponent
                          .ConfigurationBuilder.GetConfigurationObject(configId)
                          .MetadataConfiguration;

                if (metadataConfiguration == null)
                {
                    target.Result = returnResult;
                    return;
                }

                var schema = metadataConfiguration.GetSchemaVersion(documentId);
                
                var profiler = _profilerComponent.GetOperationProfiler("VersionProvider.GetNumberOfDocuments", null);
                profiler.Reset();

                var queryAnalyzer = new QueryCriteriaAnalyzer(_metadataComponent, schema);

                returnResult.NumberOfDocuments = documentProvider.GetNumberOfDocuments(queryAnalyzer.GetBeforeResolveCriteriaList(filter));
                
                profiler.TakeSnapshot();

                target.Result = returnResult;
            }
            else
            {
                target.Result = returnResult;
            }
            
            
		}
	}
}
