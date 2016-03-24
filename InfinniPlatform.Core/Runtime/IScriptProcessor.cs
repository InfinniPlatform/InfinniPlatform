using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.Runtime
{
    /// <summary>
    /// Обработчик прикладных скриптов.
    /// </summary>
    public interface IScriptProcessor
    {
        /// <summary>
        /// Выполняет прикладной скрипт.
        /// </summary>
        void InvokeScriptByType(string actionUnitType, IActionContext actionUnitContext);
    }
}