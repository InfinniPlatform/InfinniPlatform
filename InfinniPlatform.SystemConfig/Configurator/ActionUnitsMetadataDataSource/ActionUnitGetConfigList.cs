using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    /// <summary>
    ///     Получить список существующих конфигураций
    /// </summary>
    public sealed class ActionUnitGetConfigList
    {
        public void Action(IApplyResultContext target)
        {
            //получаем список всех прикладных конфигураций в системе
            target.Result = new DynamicWrapper();
            target.Result.ConfigList = QueryMetadata.QueryConfiguration(QueryMetadata.GetConfigurationShortListIql(),
                                                                        doNotCheckVersion: target.Item.DoNotCheckVersion != null
                                                                                               ? target.Item.DoNotCheckVersion
                                                                                               : false);
        }
    }
}