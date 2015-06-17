using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.RestfulApi.Extensions;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitInsertIndex
    {
        public void Action(IApplyResultContext target)
        {
            IndexedStorageExtension.SetDocument(target.Item.Item, target.Item.Configuration,target.Item.Metadata ?? string.Empty);

            target.Context.GetComponent<ILogComponent>(target.Version).GetLog().Info(
                "insert \"{0}\" document to configuration \"{1}\", type \"{2}\" ", 
                target.Item.ToString(),
                target.Item.Configuration, target.Item.Metadata);
        }
    }
}
