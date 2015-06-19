namespace InfinniPlatform.Api.Validation.ObjectValidators
{
    /// <summary>
    ///     Объект является нулевым указателем или строкой из пробелов.
    /// </summary>
    public sealed class NullOrWhiteSpaceValidator : BaseValidationOperator
    {
        protected override bool ValidateObject(object validationObject)
        {
            var propertyAsString = validationObject as string;
            return (validationObject == null) || string.IsNullOrWhiteSpace(propertyAsString);
        }
    }
}