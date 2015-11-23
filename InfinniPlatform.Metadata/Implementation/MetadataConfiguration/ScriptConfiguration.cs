using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Factories;
using InfinniPlatform.Metadata.StateMachine;
using InfinniPlatform.Metadata.StateMachine.ActionUnits;
using InfinniPlatform.Metadata.StateMachine.ValidationUnits;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Actions;
using InfinniPlatform.Sdk.Environment.Scripts;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.Metadata.Implementation.MetadataConfiguration
{
    /// <summary>
    ///     Настройки метаданных конфигурации скриптов
    /// </summary>
    public class ScriptConfiguration : IScriptConfiguration
    {
        private ExecutedScriptBuilderFactory _actionOperatorBuilderFactory;
        private IScriptFactory _scriptFactoryInstance;
        private readonly IList<ActionUnit> _actionUnits = new List<ActionUnit>();
        private readonly object _lockObject = new object();
        private readonly IScriptFactoryBuilder _scriptFactoryBuilder;
        private readonly IList<ValidationUnit> _validationUnits = new List<ValidationUnit>();

        public ScriptConfiguration(IScriptFactoryBuilder scriptFactoryBuilder)
        {
            _scriptFactoryBuilder = scriptFactoryBuilder;
        }

        public IList<ActionUnit> ActionUnits
        {
            get { return _actionUnits; }
        }

        public IList<ValidationUnit> ValidationUnits
        {
            get { return _validationUnits; }
        }

        public void RegisterActionUnitEmbedded(string unitIdentifier, IActionOperatorBuilder actionUnitBuilder)
        {
            ActionUnits.Add(new ActionUnit(unitIdentifier, actionUnitBuilder));
        }

        /// <summary>
        ///     Зарегистрировать модуль скрипта
        /// </summary>
        /// <param name="unitIdentifier">Идентификатор модуля</param>
        /// <param name="type">Класс скриптового модуля для регистрации</param>
        public void RegisterActionUnitDistributedStorage(string unitIdentifier, string type)
        {
            var factory = GetExecutedScriptBuilderFactory();
            factory.RegisterMetadata(unitIdentifier, type, "Action");
            ActionUnits.Add(new ActionUnit(unitIdentifier, factory.BuildActionOperatorBuilder(unitIdentifier)));
        }

        /// <summary>
        ///     Зарегистрировать модуль валидации
        /// </summary>
        /// <param name="unitIdentifier">Идентификатор метаданных валидатора</param>
        /// <param name="validationUnitBuilder">Конструктор валидации</param>
        public void RegisterValidationUnitEmbedded(string unitIdentifier, IValidationUnitBuilder validationUnitBuilder)
        {
            ValidationUnits.Add(new ValidationUnit(unitIdentifier, validationUnitBuilder));
        }

        /// <summary>
        ///     Зарегистрировать модуль валидации
        /// </summary>
        /// <param name="unitIdentifier">Идентификатор метаданных валидатора</param>
        /// <param name="type">Класс валидатора, который зарегистрирован для данного модуля</param>
        public void RegisterValidationUnitDistributedStorage(string unitIdentifier, string type)
        {
            GetExecutedScriptBuilderFactory().RegisterMetadata(unitIdentifier, type, "Validate");
            ValidationUnits.Add(new ValidationUnit(unitIdentifier,
                GetExecutedScriptBuilderFactory().BuildValidationOperatorBuilder(unitIdentifier)));
        }

        public void InitActionUnitStorage()
        {
            GetExecutedScriptBuilderFactory().BuildScriptProcessor();
        }

        public string ConfigurationId { get; set; }

        public IActionOperator GetAction(string unitIdentifier)
        {
            return ActionUnits
                .Where(v => v.UnitId.ToLowerInvariant() == unitIdentifier.ToLowerInvariant())
                .Select(v => v.ActionOperator)
                .FirstOrDefault();
        }

        /// <summary>
        ///     Получить оператор валидации по указанному идентификатору
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
        ///     Получить исполнитель скриптов конфигурации
        /// </summary>
        /// <returns></returns>
        public IScriptProcessor GetScriptProcessor()
        {
            return GetExecutedScriptBuilderFactory().BuildScriptProcessor();
        }

        private IScriptFactory GetScriptFactory()
        {
            lock (_lockObject)
            {
                return _scriptFactoryInstance ??
                       (_scriptFactoryInstance = _scriptFactoryBuilder.BuildScriptFactory());
            }
        }

        public ExecutedScriptBuilderFactory GetExecutedScriptBuilderFactory()
        {
            lock (_lockObject)
            {
                return _actionOperatorBuilderFactory ??
                       (_actionOperatorBuilderFactory = new ExecutedScriptBuilderFactory(GetScriptFactory()));
            }
        }
    }
}