using System;

namespace InfinniPlatform.Core.Validation.ObjectValidators
{
    /// <summary>
    ///     Объект оканчивается заданной подстрокой.
    /// </summary>
    public sealed class EndsWithValidator : BaseValidationOperator
    {
        public string Value { get; set; }

        protected override bool ValidateObject(object validationObject)
        {
            var targetAsString = validationObject as string;
            return (targetAsString != null) && targetAsString.EndsWith(Value, StringComparison.OrdinalIgnoreCase);
        }
    }
}