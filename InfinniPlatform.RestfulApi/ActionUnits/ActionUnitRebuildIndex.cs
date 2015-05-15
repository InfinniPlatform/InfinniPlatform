using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.RestfulApi.Extensions;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitRebuildIndex
    {
        public void Action(IApplyResultContext target)
        {
            IndexedStorageExtension.RebuildIndex(target.Item.Configuration, target.Item.Metadata ?? string.Empty);

            target.Context.GetComponent<ILogComponent>().GetLog().Info("Configuration \"{0}\" type index \"{1}\" recreated", target.Item.Configuration,target.Item.Metadata);
        }
    }
}
