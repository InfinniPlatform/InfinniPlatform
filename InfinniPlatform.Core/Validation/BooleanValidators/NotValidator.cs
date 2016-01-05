using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.Core.Validation.BooleanValidators
{
    /// <summary>
    ///     Объект не должен удовлетворять заданному условию.
    /// </summary>
    public sealed class NotValidator : BaseValidationOperator
    {
        public IValidationOperator Operator { get; set; }

        protected override bool ValidateObject(object validationObject)
        {
            return (Operator == null) || (Operator.Validate(validationObject) == false);
        }
    }
}