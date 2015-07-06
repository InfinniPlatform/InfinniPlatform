using System;
using InfinniPlatform.Api.Validation.BooleanValidators;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.Api.Validation.ValidationBuilders
{
    public sealed class ObjectCompositeValidationBuilder
    {
        public ObjectCompositeValidationBuilder(string property = null)
        {
            Property = property;
        }

        public string Property { get; private set; }
        public IValidationOperator Operator { get; private set; }

        /// <summary>
        ///     Добавить правило логического сложения для текущего объекта.
        /// </summary>
        public void Or(Action<ObjectValidationBuilder> buildAction)
        {
            var validationOperator = new OrValidator {Property = Property};
            buildAction(new ObjectValidationBuilder(validationOperator));

            Operator = validationOperator;
        }

        /// <summary>
        ///     Добавить правило логического умножения для текущего объекта.
        /// </summary>
        public void And(Action<ObjectValidationBuilder> buildAction)
        {
            var validationOperator = new AndValidator {Property = Property};
            buildAction(new ObjectValidationBuilder(validationOperator));

            Operator = validationOperator;
        }
    }
}