using InfinniPlatform.ContextComponents;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    ///     Обновление кэша метаданных при изменении объектов в хранилище
    /// </summary>
    public sealed class ActionUnitRefreshMetadataCache
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.IsElementDeleted != null &&
                target.Item.IsElementDeleted == true)
            {
                target.Context.GetComponent<MetadataComponent>().DeleteMetadata(target.Item.ConfigId,
                    target.Item.DocumentId,
                    target.Item.MetadataType,
                    target.Item.MetadataName);
            }
            else
            {
                target.Context.GetComponent<MetadataComponent>().UpdateMetadata(target.Item.ConfigId,
                    target.Item.DocumentId,
                    target.Item.MetadataType,
                    target.Item.MetadataName);
            }
        }
    }
}