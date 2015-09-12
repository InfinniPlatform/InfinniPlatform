using System;

namespace InfinniPlatform.Api.Validation.ObjectValidators
{
    /// <summary>
    ///     Объект меньше или равен заданного объекта.
    /// </summary>
    public sealed class LessThanOrEqualValidator : BaseValidationOperator
    {
        public object Value { get; set; }

        protected override bool ValidateObject(object validationObject)
        {
            return (validationObject != null) &&
                   (dynamic) validationObject <= (dynamic) Convert.ChangeType(Value, validationObject.GetType());
        }
    }
}