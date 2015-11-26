namespace InfinniPlatform.Sdk.Environment.Scripts
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
        /// <param name="actionUnitContext">Контекст прикладного скрипта.</param>
        void InvokeScript(string actionUnitId, object actionUnitContext);
    }
}