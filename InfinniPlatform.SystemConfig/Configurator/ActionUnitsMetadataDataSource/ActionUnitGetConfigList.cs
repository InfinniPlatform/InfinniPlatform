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
	        target.Result.ConfigList = QueryMetadata.QueryConfiguration(QueryMetadata.GetConfigurationShortListIql());
	        //new DocumentApi().GetDocument("systemconfig", "metadata", null, 0, 10000);
	    }
    }
}
