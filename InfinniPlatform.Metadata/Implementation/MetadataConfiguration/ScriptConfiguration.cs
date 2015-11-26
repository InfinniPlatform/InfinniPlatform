using System;
using System.Collections.Generic;

using InfinniPlatform.Metadata.StateMachine;
using InfinniPlatform.Metadata.StateMachine.ActionUnits;
using InfinniPlatform.Metadata.StateMachine.ValidationUnits;
using InfinniPlatform.Runtime;
using InfinniPlatform.Sdk.Environment.Actions;
using InfinniPlatform.Sdk.Environment.Scripts;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.Metadata.Implementation.MetadataConfiguration
{
    /// <summary>
    /// Настройки метаданных конфигурации скриптов
    /// </summary>
    internal sealed class ScriptConfiguration : IScriptConfiguration
    {
        public ScriptConfiguration(IScriptMetadataProvider scriptMetadataProvider, IScriptProcessor scriptProcessor)
        {
            _scriptProcessor = scriptProcessor;
            _executedScriptBuilderFactory = new ExecutedScriptBuilderFactory(scriptMetadataProvider, scriptProcessor);
            _actionUnits = new Dictionary<string, ActionUnit>(StringComparer.OrdinalIgnoreCase);
            _validationUnits = new Dictionary<string, ValidationUnit>(StringComparer.OrdinalIgnoreCase);
        }


        private readonly ExecutedScriptBuilderFactory _executedScriptBuilderFactory;
        private readonly IScriptProcessor _scriptProcessor;
        private readonly Dictionary<string, ActionUnit> _actionUnits;
        private readonly Dictionary<string, ValidationUnit> _validationUnits;


        public void RegisterActionUnitEmbedded(string unitIdentifier, IActionOperatorBuilder actionUnitBuilder)
        {
            _actionUnits[unitIdentifier] = new ActionUnit(unitIdentifier, actionUnitBuilder);
        }

        public void RegisterActionUnitDistributedStorage(string unitIdentifier, string type)
        {
            _executedScriptBuilderFactory.RegisterMetadata(unitIdentifier, type, "Action");
            _actionUnits[unitIdentifier] = new ActionUnit(unitIdentifier, _executedScriptBuilderFactory.BuildActionOperatorBuilder(unitIdentifier));
        }


        public void RegisterValidationUnitEmbedded(string unitIdentifier, IValidationUnitBuilder validationUnitBuilder)
        {
            _validationUnits[unitIdentifier] = new ValidationUnit(unitIdentifier, validationUnitBuilder);
        }

        public void RegisterValidationUnitDistributedStorage(string unitIdentifier, string type)
        {
            _executedScriptBuilderFactory.RegisterMetadata(unitIdentifier, type, "Validate");
            _validationUnits[unitIdentifier] = new ValidationUnit(unitIdentifier, _executedScriptBuilderFactory.BuildValidationOperatorBuilder(unitIdentifier));
        }


        public IActionOperator GetAction(string unitIdentifier)
        {
            ActionUnit actionUnit;

            return _actionUnits.TryGetValue(unitIdentifier, out actionUnit) ? actionUnit.ActionOperator : null;
        }

        public IValidationOperator GetValidator(string unitIdentifier)
        {
            ValidationUnit actionUnit;

            return _validationUnits.TryGetValue(unitIdentifier, out actionUnit) ? actionUnit.ValidationOperator : null;
        }


        public IScriptProcessor GetScriptProcessor()
        {
            return _scriptProcessor;
        }
    }
}