namespace InfinniPlatform.Core.Validation.ObjectValidators
{
    /// <summary>
    ///     Объект является нулевым указателем.
    /// </summary>
    public sealed class NullValidator : BaseValidationOperator
    {
        protected override bool ValidateObject(object validationObject)
        {
            return ReferenceEquals(validationObject, null);
        }
    }
}