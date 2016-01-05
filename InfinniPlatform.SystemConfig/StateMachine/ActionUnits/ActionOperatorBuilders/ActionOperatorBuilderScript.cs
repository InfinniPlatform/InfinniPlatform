using InfinniPlatform.Sdk.Environment.Actions;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.SystemConfig.StateMachine.ActionUnits.ActionOperatorBuilders
{
    public sealed class ActionOperatorBuilderScript : IActionOperatorBuilder
    {
        private readonly IScriptProcessor _scriptProcessor;
        private readonly string _unitIdentifier;

        public ActionOperatorBuilderScript(IScriptProcessor scriptProcessor, string unitIdentifier)
        {
            _scriptProcessor = scriptProcessor;
            _unitIdentifier = unitIdentifier;
        }

        public IActionOperator BuildActionOperator()
        {
            return new ActionOperator(_unitIdentifier,
                context => _scriptProcessor.InvokeScript(_unitIdentifier, context));
        }
    }
}