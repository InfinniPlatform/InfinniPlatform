using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.ContextComponents
{
    public sealed class ScriptRunnerComponent : IScriptRunnerComponent
    {
        public ScriptRunnerComponent(IScriptProcessor scriptProcessor)
        {
            _scriptProcessor = scriptProcessor;
        }


        private readonly IScriptProcessor _scriptProcessor;


        public void InvokeScript(string actionUnitId, object actionUnitContext)
        {
            _scriptProcessor.InvokeScript(actionUnitId, actionUnitContext);
        }
    }
}