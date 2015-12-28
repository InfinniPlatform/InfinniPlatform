using System.Collections.Generic;

using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitGetNumberOfDocuments
    {
        public ActionUnitGetNumberOfDocuments(IConfigurationMediatorComponent configurationMediatorComponent,
                                              IMetadataComponent metadataComponent,
                                              IVersionProvider versionProvider)
        {
            _configurationMediatorComponent = configurationMediatorComponent;
            _metadataComponent = metadataComponent;
            _versionProvider = versionProvider;
        }

        private readonly IConfigurationMediatorComponent _configurationMediatorComponent;
        private readonly IMetadataComponent _metadataComponent;
        private readonly IVersionProvider _versionProvider;

        public void Action(IApplyContext target)
        {
            dynamic returnResult = new DynamicWrapper();
            returnResult.NumberOfDocuments = 0;
            returnResult.IsValid = true;

            string configId = target.Item.Configuration;
            string documentId = target.Item.Metadata;
            IEnumerable<dynamic> filter = target.Item.Filter;

            var metadataConfiguration = _configurationMediatorComponent.ConfigurationBuilder.GetConfigurationObject(configId).MetadataConfiguration;

            if (metadataConfiguration == null)
            {
                target.Result = returnResult;
                return;
            }

            var schema = metadataConfiguration.GetSchemaVersion(documentId);

            var queryAnalyzer = new QueryCriteriaAnalyzer(_metadataComponent, schema);

            returnResult.NumberOfDocuments = _versionProvider.GetNumberOfDocuments(queryAnalyzer.GetBeforeResolveCriteriaList(filter));

            target.Result = returnResult;
        }
    }
}