using System;
using InfinniPlatform.Api.Validation.BooleanValidators;
using InfinniPlatform.Api.Validation.CollectionValidators;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.Api.Validation.ValidationBuilders
{
    public sealed class CollectionValidationBuilder
    {
        private readonly CompositeValidator _compositeValidator;

        public CollectionValidationBuilder(CompositeValidator compositeValidator)
        {
            _compositeValidator = compositeValidator;
        }

        /// <summary>
        ///     Добавить правило проверки текущей коллекции.
        /// </summary>
        public CollectionValidationBuilder Predicate(IValidationOperator predicate)
        {
            _compositeValidator.Add(predicate);

            return this;
        }

        /// <summary>
        ///     Добавить правило проверки текущей коллекции, что все ее элементы удолетворяет условиям.
        /// </summary>
        public CollectionValidationBuilder All(Action<ObjectCompositeValidationBuilder> buildAction)
        {
            var itemBuilder = new ObjectCompositeValidationBuilder();
            buildAction(itemBuilder);

            _compositeValidator.Add(new AllCollectionValidator {Operator = itemBuilder.Operator});

            return this;
        }

        /// <summary>
        ///     Добавить правило проверки текущей коллекции, что хотя бы один из ее элементов удолетворяет условиям.
        /// </summary>
        public CollectionValidationBuilder Any(Action<ObjectCompositeValidationBuilder> buildAction)
        {
            var itemBuilder = new ObjectCompositeValidationBuilder();
            buildAction(itemBuilder);

            _compositeValidator.Add(new AnyCollectionValidator {Operator = itemBuilder.Operator});

            return this;
        }

        /// <summary>
        ///     Добавить правило логического сложения для текущей коллекции.
        /// </summary>
        public CollectionValidationBuilder Or(Action<CollectionValidationBuilder> buildAction)
        {
            var compositeValidationBuilder = new CollectionCompositeValidationBuilder();
            compositeValidationBuilder.Or(buildAction);

            _compositeValidator.Add(compositeValidationBuilder.Operator);

            return this;
        }

        /// <summary>
        ///     Добавить правило логического умножения для текущей коллекции.
        /// </summary>
        public CollectionValidationBuilder And(Action<CollectionValidationBuilder> buildAction)
        {
            var compositeValidationBuilder = new CollectionCompositeValidationBuilder();
            compositeValidationBuilder.And(buildAction);

            _compositeValidator.Add(compositeValidationBuilder.Operator);

            return this;
        }
    }
}