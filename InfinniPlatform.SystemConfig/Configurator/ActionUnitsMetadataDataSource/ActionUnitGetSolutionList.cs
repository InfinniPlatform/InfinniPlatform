using System;

using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsMetadataDataSource
{
    /// <summary>
    /// Получить список метаданных решений
    /// </summary>
    [Obsolete]
    public sealed class ActionUnitGetSolutionList
    {
        public void Action(IApplyResultContext target)
        {
            target.Result = new DynamicWrapper();
        }
    }
}