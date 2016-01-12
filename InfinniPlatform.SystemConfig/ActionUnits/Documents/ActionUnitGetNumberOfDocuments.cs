using System.Collections.Generic;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.ActionUnits.Documents
{
    public sealed class ActionUnitGetNumberOfDocuments
    {
        public ActionUnitGetNumberOfDocuments(IConfigurationObjectBuilder configurationObjectBuilder,
                                              IMetadataComponent metadataComponent,
                                              IVersionProvider versionProvider)
        {
            _configurationObjectBuilder = configurationObjectBuilder;
            _metadataComponent = metadataComponent;
            _versionProvider = versionProvider;
        }

        private readonly IConfigurationObjectBuilder _configurationObjectBuilder;
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

            var metadataConfiguration = _configurationObjectBuilder.GetConfigurationObject(configId).MetadataConfiguration;

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