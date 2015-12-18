using System;

using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    /// <summary>
    /// Получить список существующих конфигураций
    /// </summary>
    [Obsolete]
    public sealed class ActionUnitGetConfigList
    {
        public void Action(IApplyResultContext target)
        {
            //получаем список всех прикладных конфигураций в системе
            target.Result = new DynamicWrapper();
        }
    }
}