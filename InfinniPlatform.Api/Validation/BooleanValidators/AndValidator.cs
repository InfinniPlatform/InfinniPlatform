using System.Linq;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Validation.BooleanValidators
{
    /// <summary>
    ///     Объект должен удовлетворять всем заданным условиям.
    /// </summary>
    public sealed class AndValidator : CompositeValidator
    {
        public override bool Validate(object validationObject, ValidationResult validationResult = null,
            string parentProperty = null)
        {
            var isValid = true;
            var propertyValue = validationObject.GetProperty(Property);

            ValidationResult itemValidationResult = null;

            if (Operators != null && Operators.Any())
            {
                itemValidationResult = new ValidationResult();

                foreach (var validationOperator in Operators)
                {
                    isValid &= validationOperator.Validate(propertyValue, itemValidationResult,
                        ValidationExtensions.CombineProperties(parentProperty, Property));
                }
            }

            validationResult.SetValidationResult(isValid, itemValidationResult);

            return isValid;
        }
    }
}