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
        /// <param name="actionUnitId">Идентификатор прикладного скрипта.</param>
        /// <param name="actionUnitContext">Контекст прикладного обработчика</param>
        void InvokeScript(string actionUnitId, IActionContext actionUnitContext);

        /// <summary>
        /// Выполняет прикладной скрипт.
        /// </summary>
        /// <param name="actionUnitType">Тип прикладного скрипта.</param>
        /// <param name="actionUnitContext">Контекст прикладного обработчика</param>
        void InvokeScriptByType(string actionUnitType, IActionContext actionUnitContext);
    }
}