using System.Linq;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Validation.BooleanValidators
{
    /// <summary>
    ///     Объект должен удовлетворять хотя бы одному из заданных условий.
    /// </summary>
    public sealed class OrValidator : CompositeValidator
    {
        public override bool Validate(object validationObject, ValidationResult validationResult = null,
            string parentProperty = null)
        {
            var isValid = false;
            var propertyValue = validationObject.GetProperty(Property);

            ValidationResult itemValidationResult = null;

            if (Operators != null && Operators.Any())
            {
                itemValidationResult = new ValidationResult();

                foreach (var validationOperator in Operators)
                {
                    if (validationOperator.Validate(propertyValue, itemValidationResult,
                        ValidationExtensions.CombineProperties(parentProperty, Property)))
                    {
                        isValid = true;
                        break;
                    }
                }
            }

            validationResult.SetValidationResult(isValid, itemValidationResult);

            return isValid;
        }
    }
}