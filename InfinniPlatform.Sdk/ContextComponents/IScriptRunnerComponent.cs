namespace InfinniPlatform.Sdk.ContextComponents
{
    /// <summary>
    /// Исполнитель скриптов из глобального контекста
    /// </summary>
    public interface IScriptRunnerComponent
    {
        void InvokeScript(string actionUnitId, object actionUnitContext);
    }
}