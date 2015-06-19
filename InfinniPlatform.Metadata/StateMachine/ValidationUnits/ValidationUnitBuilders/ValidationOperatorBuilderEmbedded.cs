using System;
using InfinniPlatform.Api.Validation;

namespace InfinniPlatform.Metadata.StateMachine.ValidationUnits.ValidationUnitBuilders
{
    /// <summary>
    ///     Билдер для создания предварительно указанных в Design-time и скомпилированных
    ///     валидаторов. Поскольку без перекомпиляции исходников невозможно
    ///     в "горячем режиме" произвести замену таких валидаторов (в отличие, например,
    ///     от скриптовых валидаторов), то создавать такие валидаторы будем только один раз,
    ///     при первом вызове.
    ///     Валидатор является иммутабельным типом, не имеющим и не сохраняющим состояния.
    ///     Добавление состояния в валидаторе недопустимо.
    /// </summary>
    public sealed class ValidationOperatorBuilderEmbedded : IValidationUnitBuilder
    {
        private IValidationOperator _validatorInstance;
        private readonly Type _validatorType;

        public ValidationOperatorBuilderEmbedded(Type validatorType)
        {
            _validatorType = validatorType;
        }

        public IValidationOperator BuildValidationUnit()
        {
            if (_validatorInstance != null)
            {
                return _validatorInstance;
            }

            var validatorInstance = Activator.CreateInstance(_validatorType);
            if (!(validatorInstance is IValidationOperator))
            {
                throw new ArgumentException("registered type is not IValidationOperator");
            }
            _validatorInstance = (IValidationOperator) validatorInstance;
            return _validatorInstance;
        }
    }
}