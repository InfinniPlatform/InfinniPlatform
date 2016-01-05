using System;

using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.Core.Validation.ValidationBuilders
{
    public static class ValidationBuilder
    {
        /// <summary>
        ///     Построить правила проверки для объекта.
        /// </summary>
        public static IValidationOperator ForObject(Action<ObjectCompositeValidationBuilder> buildAction)
        {
            var builder = new ObjectCompositeValidationBuilder();
            buildAction(builder);

            return builder.Operator;
        }

        /// <summary>
        ///     Построить правила проверки для объекта.
        /// </summary>
        public static IValidationOperator ForObject<T>(Action<ObjectCompositeValidationGenericBuilder<T>> buildAction)
        {
            var builder = new ObjectCompositeValidationGenericBuilder<T>();
            buildAction(builder);

            return builder.Operator;
        }

        /// <summary>
        ///     Построить правила проверки для коллекции.
        /// </summary>
        public static IValidationOperator ForCollection(Action<CollectionCompositeValidationBuilder> buildAction)
        {
            var builder = new CollectionCompositeValidationBuilder();
            buildAction(builder);

            return builder.Operator;
        }

        /// <summary>
        ///     Построить правила проверки для коллекции.
        /// </summary>
        public static IValidationOperator ForCollection<T>(
            Action<CollectionCompositeValidationGenericBuilder<T>> buildAction)
        {
            var builder = new CollectionCompositeValidationGenericBuilder<T>();
            buildAction(builder);

            return builder.Operator;
        }
    }
}