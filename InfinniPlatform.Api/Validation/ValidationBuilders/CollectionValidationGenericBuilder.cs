using System;
using InfinniPlatform.Api.Validation.BooleanValidators;
using InfinniPlatform.Api.Validation.CollectionValidators;

namespace InfinniPlatform.Api.Validation.ValidationBuilders
{
    public sealed class CollectionValidationGenericBuilder<T>
    {
        private readonly CompositeValidator _validationOperator;

        public CollectionValidationGenericBuilder(CompositeValidator validationOperator)
        {
            _validationOperator = validationOperator;
        }

        /// <summary>
        ///     Добавить правило проверки текущей коллекции.
        /// </summary>
        public CollectionValidationGenericBuilder<T> Predicate(IValidationOperator predicate)
        {
            _validationOperator.Add(predicate);

            return this;
        }

        /// <summary>
        ///     Добавить правило проверки текущей коллекции, что все ее элементы удолетворяет условиям.
        /// </summary>
        public CollectionValidationGenericBuilder<T> Any(Action<ObjectCompositeValidationGenericBuilder<T>> buildAction)
        {
            var itemBuilder = new ObjectCompositeValidationGenericBuilder<T>();
            buildAction(itemBuilder);

            _validationOperator.Add(new AnyCollectionValidator {Operator = itemBuilder.Operator});

            return this;
        }

        /// <summary>
        ///     Добавить правило проверки текущей коллекции, что хотя бы один из ее элементов удолетворяет условиям.
        /// </summary>
        public CollectionValidationGenericBuilder<T> All(Action<ObjectCompositeValidationGenericBuilder<T>> buildAction)
        {
            var itemBuilder = new ObjectCompositeValidationGenericBuilder<T>();
            buildAction(itemBuilder);

            _validationOperator.Add(new AllCollectionValidator {Operator = itemBuilder.Operator});

            return this;
        }

        /// <summary>
        ///     Добавить правило логического сложения для текущей коллекции.
        /// </summary>
        public CollectionValidationGenericBuilder<T> Or(Action<CollectionValidationGenericBuilder<T>> buildAction)
        {
            var compositeValidationBuilder = new CollectionCompositeValidationGenericBuilder<T>();
            compositeValidationBuilder.Or(buildAction);

            _validationOperator.Add(compositeValidationBuilder.Operator);

            return this;
        }

        /// <summary>
        ///     Добавить правило логического умножения для текущей коллекции.
        /// </summary>
        public CollectionValidationGenericBuilder<T> And(Action<CollectionValidationGenericBuilder<T>> buildAction)
        {
            var compositeValidationBuilder = new CollectionCompositeValidationGenericBuilder<T>();
            compositeValidationBuilder.And(buildAction);

            _validationOperator.Add(compositeValidationBuilder.Operator);

            return this;
        }
    }
}