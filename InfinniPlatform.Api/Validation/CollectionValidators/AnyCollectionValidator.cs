using System.Globalization;
using System.Linq;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.Api.Validation.CollectionValidators
{
    /// <summary>
    ///     Один из элементов коллекции удовлетворяют заданному условию.
    /// </summary>
    public sealed class AnyCollectionValidator : IValidationOperator
    {
        public IValidationOperator Operator { get; set; }

        public bool Validate(object validationObject, ValidationResult validationResult = null,
            string parentProperty = null)
        {
            var isValid = false;
            var collection = validationObject.TryCastToEnumerable();

            ValidationResult itemValidationResult = null;

            if (collection != null && collection.Any())
            {
                itemValidationResult = new ValidationResult();

                var itemIndex = 0;

                foreach (var item in collection)
                {
                    if (Operator.Validate(item, itemValidationResult,
                        ValidationExtensions.CombineProperties(parentProperty,
                            itemIndex.ToString(CultureInfo.InvariantCulture))))
                    {
                        isValid = true;
                        break;
                    }

                    ++itemIndex;
                }
            }

            validationResult.SetValidationResult(isValid, itemValidationResult);

            return isValid;
        }
    }
}