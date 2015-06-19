using System;

namespace InfinniPlatform.Api.Validation.ObjectValidators
{
    /// <summary>
    ///     Объект является абсолютным URI.
    /// </summary>
    public sealed class AbsoluteUriValidator : BaseValidationOperator
    {
        protected override bool ValidateObject(object validationObject)
        {
            Uri uri;
            return (validationObject != null) && Uri.TryCreate(validationObject.ToString(), UriKind.Absolute, out uri);
        }
    }
}