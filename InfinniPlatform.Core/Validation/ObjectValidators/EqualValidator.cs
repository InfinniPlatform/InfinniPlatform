using System;

namespace InfinniPlatform.Core.Validation.ObjectValidators
{
    /// <summary>
    ///     Объект равен заданному объекту.
    /// </summary>
    public sealed class EqualValidator : BaseValidationOperator
    {
        public object Value { get; set; }

        protected override bool ValidateObject(object validationObject)
        {
            return Equals(validationObject, Value) ||
                   (validationObject != null && Value != null &&
                    Equals(validationObject, Convert.ChangeType(Value, validationObject.GetType())));
        }
    }
}