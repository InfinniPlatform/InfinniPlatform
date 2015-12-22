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
        public ActionUnitGetNumberOfDocuments(IConfigurationMediatorComponent configurationMediatorComponent,
                                              IMetadataComponent metadataComponent,
                                              InprocessDocumentComponent documentComponent)
        {
            _configurationMediatorComponent = configurationMediatorComponent;
            _metadataComponent = metadataComponent;
            _documentComponent = documentComponent;
        }

        private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
        private readonly InprocessDocumentComponent _documentComponent;
        private readonly IMetadataComponent _metadataComponent;

        public void Action(IApplyContext target)
        {
            dynamic returnResult = new DynamicWrapper();
            returnResult.NumberOfDocuments = 0;
            returnResult.IsValid = true;

            string configId = target.Item.Configuration;
            string documentId = target.Item.Metadata;
            IEnumerable<dynamic> filter = target.Item.Filter;

            var documentProvider = _documentComponent.GetDocumentProvider(configId, documentId);

            if (documentProvider != null)
            {
                var metadataConfiguration = _configurationMediatorComponent.ConfigurationBuilder.GetConfigurationObject(configId).MetadataConfiguration;

                if (metadataConfiguration == null)
                {
                    target.Result = returnResult;
                    return;
                }

                var schema = metadataConfiguration.GetSchemaVersion(documentId);

                var queryAnalyzer = new QueryCriteriaAnalyzer(_metadataComponent, schema);

                returnResult.NumberOfDocuments = documentProvider.GetNumberOfDocuments(queryAnalyzer.GetBeforeResolveCriteriaList(filter));

                target.Result = returnResult;
            }
            else
            {
                target.Result = returnResult;
            }
        }
    }
}