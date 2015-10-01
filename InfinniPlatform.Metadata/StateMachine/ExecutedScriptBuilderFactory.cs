using System;
using InfinniPlatform.Factories;
using InfinniPlatform.Metadata.StateMachine.ActionUnits.ActionOperatorBuilders;
using InfinniPlatform.Metadata.StateMachine.ValidationUnits.ValidationUnitBuilders;
using InfinniPlatform.Runtime;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.Metadata.StateMachine
{
    /// <summary>
    ///     Фабрика конструкторов скриптовых модулей
    /// </summary>
    public sealed class ExecutedScriptBuilderFactory
    {
        private IScriptProcessor _scriptProcessor;
        private readonly IScriptFactory _scriptFactory;
        private readonly IScriptMetadataProvider _scriptMetadataProvider;

        public ExecutedScriptBuilderFactory(IScriptFactory scriptFactory)
        {
            _scriptFactory = scriptFactory;
            _scriptMetadataProvider = _scriptFactory.BuildScriptMetadataProvider();
        }

        public IScriptProcessor BuildScriptProcessor()
        {
            return _scriptProcessor ?? (_scriptProcessor = _scriptFactory.BuildScriptProcessor());
        }

        public ActionOperatorBuilderScript BuildActionOperatorBuilder(string unitIdentifier)
        {
            return new ActionOperatorBuilderScript(BuildScriptProcessor(), unitIdentifier);
        }

        public ValidationOperatorBuilderScript BuildValidationOperatorBuilder(string unitIdentifier)
        {
            return new ValidationOperatorBuilderScript(BuildScriptProcessor(), unitIdentifier);
        }

        public void RegisterMetadata(string unitIdentifier, string type, string action)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentException("Type should not be empty.");
            }

            _scriptMetadataProvider.SetScriptMetadata(new ScriptMetadata
            {
                Identifier = unitIdentifier,
                Type = type,
                Method = action
            });
        }
    }
}