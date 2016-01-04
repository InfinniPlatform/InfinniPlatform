using InfinniPlatform.Sdk.Environment.Scripts;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.SystemConfig.Metadata.StateMachine.ValidationUnits.ValidationUnitBuilders
{
    public sealed class ValidationOperatorBuilderScript : IValidationUnitBuilder
    {
        private readonly IScriptProcessor _scriptProcessor;
        private readonly string _unitIdentifier;

        public ValidationOperatorBuilderScript(IScriptProcessor scriptProcessor, string unitIdentifier)
        {
            _scriptProcessor = scriptProcessor;
            _unitIdentifier = unitIdentifier;
        }

        public IValidationOperator BuildValidationUnit()
        {
            return new ValidationOperator(context => { _scriptProcessor.InvokeScript(_unitIdentifier, context); });
        }
    }
}