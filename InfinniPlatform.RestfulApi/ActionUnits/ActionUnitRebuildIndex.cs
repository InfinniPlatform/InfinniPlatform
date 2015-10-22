using InfinniPlatform.RestfulApi.Extensions;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitRebuildIndex
    {
        public void Action(IApplyResultContext target)
        {
            string indexName = target.Item.Configuration;
            string indexTypeName = target.Item.Metadata ?? string.Empty;

            IndexedStorageExtension.RebuildIndex(indexName, indexTypeName);
        }
    }
}