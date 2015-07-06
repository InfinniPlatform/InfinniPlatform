using InfinniPlatform.RestfulApi.Extensions;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitInsertIndex
    {
        public void Action(IApplyResultContext target)
        {
            IndexedStorageExtension.SetDocument(target.Item.Item, target.Item.Configuration,
                                                target.Item.Metadata ?? string.Empty);

            target.Context.GetComponent<ILogComponent>().GetLog().Info(
                "insert \"{0}\" document to configuration \"{1}\", type \"{2}\" ",
                target.Item.ToString(),
                target.Item.Configuration, target.Item.Metadata);
        }
    }
}