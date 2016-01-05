using System.Globalization;
using System.Linq;

using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.Core.Validation.CollectionValidators
{
    /// <summary>
    ///     Все элементы коллекции удовлетворяют заданному условию.
    /// </summary>
    public sealed class AllCollectionValidator : IValidationOperator
    {
        public IValidationOperator Operator { get; set; }

        public bool Validate(object validationObject, ValidationResult validationResult = null,
            string parentProperty = null)
        {
            var isValid = true;
            var collection = validationObject.TryCastToEnumerable();

            ValidationResult itemValidationResult = null;

            if (collection != null && collection.Any())
            {
                itemValidationResult = new ValidationResult();

                var itemIndex = 0;

                foreach (var item in collection)
                {
                    isValid &= Operator.Validate(item, itemValidationResult,
                        ValidationExtensions.CombineProperties(parentProperty,
                            itemIndex.ToString(CultureInfo.InvariantCulture)));

                    ++itemIndex;
                }
            }

            validationResult.SetValidationResult(isValid, itemValidationResult);

            return isValid;
        }
    }
}