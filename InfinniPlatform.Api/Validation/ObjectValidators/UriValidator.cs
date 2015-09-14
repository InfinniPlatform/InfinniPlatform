using System;

namespace InfinniPlatform.Api.Validation.ObjectValidators
{
    /// <summary>
    ///     Объект является URI.
    /// </summary>
    public sealed class UriValidator : BaseValidationOperator
    {
        protected override bool ValidateObject(object validationObject)
        {
            Uri uri;
            return (validationObject != null) &&
                   Uri.TryCreate(validationObject.ToString(), UriKind.RelativeOrAbsolute, out uri);
        }
    }
}