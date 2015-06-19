using System;

namespace InfinniPlatform.Api.Validation.ObjectValidators
{
    /// <summary>
    ///     Объект является глобально уникальным идентификатором (GUID).
    /// </summary>
    public sealed class GuidValidator : BaseValidationOperator
    {
        protected override bool ValidateObject(object validationObject)
        {
            Guid guid;
            return (validationObject != null) && Guid.TryParse(validationObject.ToString(), out guid);
        }
    }
}