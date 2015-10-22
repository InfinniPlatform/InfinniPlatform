using InfinniPlatform.RestfulApi.Extensions;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitIndexExists
    {
        public void Action(IApplyResultContext target)
        {
            var indexName = target.Item.Configuration;
            var indexType = target.Item.Metadata ?? string.Empty;
            var indexExists = IndexedStorageExtension.IndexExists(indexName, indexType);

            dynamic result = new DynamicWrapper();
            result.IndexExists = indexExists;
            result.IsValid = true;
            result.ValidationMessage = "Index status successfully checked";
            target.Result = result;
        }
    }
}