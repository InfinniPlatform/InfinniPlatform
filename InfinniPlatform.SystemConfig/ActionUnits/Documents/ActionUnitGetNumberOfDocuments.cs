using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.SystemConfig.ActionUnits.Documents
{
    public sealed class ActionUnitGetNumberOfDocuments
    {
        public ActionUnitGetNumberOfDocuments(IDocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly IDocumentApi _documentApi;

        public void Action(IApplyContext target)
        {
            string configuration = target.Item.Configuration;
            string documentType = target.Item.Metadata;
            object filter = target.Item.Filter;

            var filterCriterias = JsonObjectSerializer.Default.ConvertFromDynamic<FilterCriteria[]>(filter);

            var numberOfDocuments = _documentApi.GetNumberOfDocuments(configuration, documentType, filterCriterias);

            target.Result = new DynamicWrapper();
            target.Result.IsValid = true;
            target.Result.NumberOfDocuments = numberOfDocuments;
        }
    }
}