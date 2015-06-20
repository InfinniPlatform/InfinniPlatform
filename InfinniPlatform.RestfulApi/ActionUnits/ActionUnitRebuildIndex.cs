using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.RestfulApi.Extensions;
using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.RestfulApi.ActionUnits
{
    public sealed class ActionUnitRebuildIndex
    {
        public void Action(IApplyResultContext target)
        {
            IndexedStorageExtension.RebuildIndex(target.Item.Configuration, target.Item.Metadata ?? string.Empty);

            target.Context.GetComponent<ILogComponent>(target.Version)
                  .GetLog()
                  .Info("Configuration \"{0}\" type index \"{1}\" recreated", target.Item.Configuration,
                        target.Item.Metadata);
        }
    }
}