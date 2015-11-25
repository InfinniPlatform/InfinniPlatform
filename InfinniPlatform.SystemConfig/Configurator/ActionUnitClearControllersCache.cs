using System;

using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    /// Очищает кэш зарегистрированных контроллеров
    /// (вызов необходим для добавления пользовательского контроллера)
    /// </summary>
    [Obsolete]
    public sealed class ActionUnitClearControllersCache
    {
        public void Action(IApplyContext target)
        {
        }
    }
}