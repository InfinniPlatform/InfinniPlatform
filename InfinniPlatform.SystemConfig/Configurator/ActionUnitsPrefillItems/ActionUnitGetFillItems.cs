using System.Collections.Generic;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.SystemConfig.Installers;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsPrefillItems
{
    /// <summary>
    ///     Получение списка модулей предзаполнения
    /// </summary>
    internal sealed class ActionUnitGetFillItems
    {
        public void Action(IApplyResultContext target)
        {
            var result = new List<string>();

            target.Result = result;

            var prefilledItems = new ActionUnitsEntryList(GetType().Assembly, ActionUnitsEntryList.PrefillIndex);
            foreach (var prefilledItem in prefilledItems.Entries)
            {
                result.Add(prefilledItem.Key);
            }
        }
    }
}