using System;

using InfinniPlatform.Core.Extensions;

namespace InfinniPlatform.Core.Validation.ObjectValidators
{
    /// <summary>
    ///     Объект содержит заданную подстроку.
    /// </summary>
    public sealed class ContainsValidator : BaseValidationOperator
    {
        public string Value { get; set; }

        protected override bool ValidateObject(object validationObject)
        {
            var targetAsString = validationObject as string;
            return (targetAsString != null) && targetAsString.Contains(Value, StringComparison.OrdinalIgnoreCase);
        }
    }
}