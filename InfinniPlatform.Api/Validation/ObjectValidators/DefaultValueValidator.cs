using InfinniPlatform.Api.Extensions;
using InfinniPlatform.Sdk.Application.Extensions;

namespace InfinniPlatform.Api.Validation.ObjectValidators
{
    /// <summary>
    ///     Объект является значением по умолчанию для данного типа.
    /// </summary>
    public sealed class DefaultValueValidator : BaseValidationOperator
    {
        protected override bool ValidateObject(object validationObject)
        {
            if (validationObject != null)
            {
                var defaultValue = validationObject.GetType().GetDefaultValue();
                return Equals(validationObject, defaultValue);
            }

            return true;
        }
    }
}