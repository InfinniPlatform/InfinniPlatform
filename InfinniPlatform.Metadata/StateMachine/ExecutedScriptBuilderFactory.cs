using InfinniPlatform.Metadata.StateMachine.ActionUnits.ActionOperatorBuilders;
using InfinniPlatform.Metadata.StateMachine.ValidationUnits.ValidationUnitBuilders;
using InfinniPlatform.Runtime;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.Metadata.StateMachine
{
    /// <summary>
    /// Фабрика конструкторов скриптовых модулей
    /// </summary>
    public sealed class ExecutedScriptBuilderFactory
    {
        public ExecutedScriptBuilderFactory(IScriptMetadataProvider scriptMetadataProvider, IScriptProcessor scriptProcessor)
        {
            _scriptMetadataProvider = scriptMetadataProvider;
            _scriptProcessor = scriptProcessor;
        }


        private readonly IScriptMetadataProvider _scriptMetadataProvider;
        private readonly IScriptProcessor _scriptProcessor;


        public ActionOperatorBuilderScript BuildActionOperatorBuilder(string unitIdentifier)
        {
            return new ActionOperatorBuilderScript(_scriptProcessor, unitIdentifier);
        }

        public ValidationOperatorBuilderScript BuildValidationOperatorBuilder(string unitIdentifier)
        {
            return new ValidationOperatorBuilderScript(_scriptProcessor, unitIdentifier);
        }

        public void RegisterMetadata(string unitIdentifier, string type, string action)
        {
            _scriptMetadataProvider.SetScriptMetadata(new ScriptMetadata
                                                      {
                                                          Identifier = unitIdentifier,
                                                          Type = type,
                                                          Method = action
                                                      });
        }
    }
}