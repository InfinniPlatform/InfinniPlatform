using System;
using InfinniPlatform.Api.Validation.BooleanValidators;

namespace InfinniPlatform.Api.Validation.ValidationBuilders
{
    public sealed class ObjectValidationBuilder
    {
        private readonly CompositeValidator _compositeValidator;

        public ObjectValidationBuilder(CompositeValidator compositeValidator)
        {
            _compositeValidator = compositeValidator;
        }

        /// <summary>
        ///     Добавить правило проверки текущего объекта.
        /// </summary>
        public ObjectValidationBuilder Predicate(IValidationOperator predicate)
        {
            _compositeValidator.Add(predicate);

            return this;
        }

        /// <summary>
        ///     Добавить правило проверки свойства текущего объекта.
        /// </summary>
        public ObjectValidationBuilder Property(string property, Action<ObjectCompositeValidationBuilder> buildAction)
        {
            var builder = new ObjectCompositeValidationBuilder(property);

            buildAction(builder);

            _compositeValidator.Add(builder.Operator);

            return this;
        }

        /// <summary>
        ///     Добавить правило проверки коллекции текущего объекта.
        /// </summary>
        public ObjectValidationBuilder Collection(string property,
            Action<CollectionCompositeValidationBuilder> buildAction)
        {
            var builder = new CollectionCompositeValidationBuilder(property);

            buildAction(builder);

            _compositeValidator.Add(builder.Operator);

            return this;
        }

        /// <summary>
        ///     Добавить правило логического сложения для текущего объекта.
        /// </summary>
        public ObjectValidationBuilder Or(Action<ObjectValidationBuilder> buildAction)
        {
            var compositeValidationBuilder = new ObjectCompositeValidationBuilder();
            compositeValidationBuilder.Or(buildAction);

            _compositeValidator.Add(compositeValidationBuilder.Operator);

            return this;
        }

        /// <summary>
        ///     Добавить правило логического умножения для текущего объекта.
        /// </summary>
        public ObjectValidationBuilder And(Action<ObjectValidationBuilder> buildAction)
        {
            var compositeValidationBuilder = new ObjectCompositeValidationBuilder();
            compositeValidationBuilder.And(buildAction);

            _compositeValidator.Add(compositeValidationBuilder.Operator);

            return this;
        }
    }
}