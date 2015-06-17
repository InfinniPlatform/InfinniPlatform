using System.Diagnostics;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    /// <summary>
    ///   Получить список существующих конфигураций
    /// </summary>
    public sealed class ActionUnitGetConfigList
    {
	    public void Action(IApplyResultContext target)
	    {
            //получаем список всех прикладных конфигураций в системе
            target.Result = new DynamicWrapper();
            target.Result.ConfigList = QueryMetadata.QueryConfiguration(target.Version, QueryMetadata.GetConfigurationShortListIql(), 
                target.Item.DoNotCheckVersion != null ? target.Item.DoNotCheckVersion : false);
	    }
    }
}
