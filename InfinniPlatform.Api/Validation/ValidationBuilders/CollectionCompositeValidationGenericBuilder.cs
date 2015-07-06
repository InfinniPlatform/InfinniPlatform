using System;
using InfinniPlatform.Api.Validation.BooleanValidators;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.Api.Validation.ValidationBuilders
{
    public sealed class CollectionCompositeValidationGenericBuilder<T>
    {
        public CollectionCompositeValidationGenericBuilder(string property = null)
        {
            Property = property;
        }

        public string Property { get; private set; }
        public IValidationOperator Operator { get; private set; }

        /// <summary>
        ///     Добавить правило логического сложения для текущей коллекции.
        /// </summary>
        public void Or(Action<CollectionValidationGenericBuilder<T>> buildAction)
        {
            var validationOperator = new OrValidator {Property = Property};
            buildAction(new CollectionValidationGenericBuilder<T>(validationOperator));

            Operator = validationOperator;
        }

        /// <summary>
        ///     Добавить правило логического умножения для текущей коллекции.
        /// </summary>
        public void And(Action<CollectionValidationGenericBuilder<T>> buildAction)
        {
            var validationOperator = new AndValidator {Property = Property};
            buildAction(new CollectionValidationGenericBuilder<T>(validationOperator));

            Operator = validationOperator;
        }
    }
}