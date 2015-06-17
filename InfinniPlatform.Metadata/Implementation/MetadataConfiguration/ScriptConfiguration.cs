﻿using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Actions;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Factories;
using InfinniPlatform.Metadata.StateMachine;
using InfinniPlatform.Metadata.StateMachine.ActionUnits;
using InfinniPlatform.Metadata.StateMachine.ValidationUnits;
using InfinniPlatform.Runtime;

namespace InfinniPlatform.Metadata.Implementation.MetadataConfiguration
{
    /// <summary>
    ///   Настройки метаданных конфигурации скриптов
    /// </summary>
    public class ScriptConfiguration : IScriptConfiguration
    {
        private IScriptFactory _scriptFactoryInstance;

        private readonly object _lockObject = new object();


        private IScriptFactory GetScriptFactory(string version)
        {
            lock (_lockObject)
            {
                return _scriptFactoryInstance ??
                       (_scriptFactoryInstance = _scriptFactoryBuilder.BuildScriptFactory(ModuleName, version));
            }
        }

        private ExecutedScriptBuilderFactory _actionOperatorBuilderFactory;
        private readonly IScriptFactoryBuilder _scriptFactoryBuilder;


        public ExecutedScriptBuilderFactory GetExecutedScriptBuilderFactory(string version)
        {
            lock (_lockObject)
            {
                return _actionOperatorBuilderFactory ??
                       (_actionOperatorBuilderFactory = new ExecutedScriptBuilderFactory(GetScriptFactory(version)));
            }

        }

        public ScriptConfiguration(IScriptFactoryBuilder scriptFactoryBuilder)
        {
            _scriptFactoryBuilder = scriptFactoryBuilder;
        }

        private readonly IList<ActionUnit> _actionUnits = new List<ActionUnit>();

        public IList<ActionUnit> ActionUnits
        {
            get { return _actionUnits; }
        }

        private readonly IList<ValidationUnit> _validationUnits = new List<ValidationUnit>();

        public IList<ValidationUnit> ValidationUnits
        {
            get { return _validationUnits; }
        }

        public void RegisterActionUnitEmbedded(string unitIdentifier, IActionOperatorBuilder actionUnitBuilder)
        {
            ActionUnits.Add(new ActionUnit(unitIdentifier, actionUnitBuilder));
        }

        /// <summary>
        ///   Зарегистрировать модуль скрипта
        /// </summary>
        /// <param name="unitIdentifier">Идентификатор модуля</param>
        /// <param name="type">Класс скриптового модуля для регистрации</param>
        /// <param name="version"></param>
        public void RegisterActionUnitDistributedStorage(string unitIdentifier, string type, string version)
        {
            var factory = GetExecutedScriptBuilderFactory(version);
            factory.RegisterMetadata(unitIdentifier, type, "Action");
            ActionUnits.Add(new ActionUnit(unitIdentifier, factory.BuildActionOperatorBuilder(unitIdentifier)));
        }

        /// <summary>
        ///  Зарегистрировать модуль валидации
        /// </summary>
        /// <param name="unitIdentifier">Идентификатор метаданных валидатора</param>
        /// <param name="validationUnitBuilder">Конструктор валидации</param>
        public void RegisterValidationUnitEmbedded(string unitIdentifier, IValidationUnitBuilder validationUnitBuilder)
        {
            ValidationUnits.Add(new ValidationUnit(unitIdentifier, validationUnitBuilder));
        }

        /// <summary>
        ///  Зарегистрировать модуль валидации
        /// </summary>
        /// <param name="unitIdentifier">Идентификатор метаданных валидатора</param>
        /// <param name="type">Класс валидатора, который зарегистрирован для данного модуля</param>
        /// <param name="version"></param>
        public void RegisterValidationUnitDistributedStorage(string unitIdentifier, string type, string version)
        {
            GetExecutedScriptBuilderFactory(version).RegisterMetadata(unitIdentifier, type, "Validate");
            ValidationUnits.Add(new ValidationUnit(unitIdentifier, GetExecutedScriptBuilderFactory(version).BuildValidationOperatorBuilder(unitIdentifier)));
        }

        public void InitActionUnitStorage(string version)
        {
            GetExecutedScriptBuilderFactory(version).BuildScriptProcessor();
        }

        public string ModuleName { get; set; }

        public IActionOperator GetAction(string unitIdentifier)
        {
            return ActionUnits
                .Where(v => v.UnitId.ToLowerInvariant() == unitIdentifier.ToLowerInvariant())
                .Select(v => v.ActionOperator)
                .FirstOrDefault();
        }

        /// <summary>
        ///   Получить оператор валидации по указанному идентификатору
        /// </summary>
        /// <param name="unitIdentifier">Идентификатор валидации</param>
        /// <returns>Оператор валидации</returns>
        public IValidationOperator GetValidator(string unitIdentifier)
        {
            return ValidationUnits
                .Where(v => v.UnitId.ToLowerInvariant() == unitIdentifier.ToLowerInvariant())
                .Select(v => v.ValidationOperator)
                .FirstOrDefault();
        }

        /// <summary>
        ///   Получить исполнитель скриптов конфигурации
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public IScriptProcessor GetScriptProcessor(string version)
        {
            return GetExecutedScriptBuilderFactory(version).BuildScriptProcessor();
        }
    }
}