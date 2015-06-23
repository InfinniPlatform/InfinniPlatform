using System;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Configurator.ActionUnitsPrefillItems
{
    /// <summary>
    ///     Модуль предзаполнения поля документа текущей датой
    /// </summary>
    public sealed class ActionUnitPrefillDateTimeNow
    {
        public void Action(IApplyContext target)
        {
            if (string.IsNullOrEmpty(target.Item.FieldName))
            {
                return;
            }

            target.Item.Document[target.Item.FieldName] = DateTime.Now;
            target.Result = target.Item.Document;
        }
    }
}