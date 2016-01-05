namespace InfinniPlatform.Core.Validation.ObjectValidators
{
    /// <summary>
    ///     Объект является нулевым указателем или пустой строкой.
    /// </summary>
    public sealed class NullOrEmptyValidator : BaseValidationOperator
    {
        protected override bool ValidateObject(object validationObject)
        {
            var targetAsString = validationObject as string;
            return (validationObject == null) || string.IsNullOrEmpty(targetAsString);
        }
    }
}