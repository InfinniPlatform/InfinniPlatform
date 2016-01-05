using System.Text.RegularExpressions;

namespace InfinniPlatform.Core.Validation.ObjectValidators
{
    /// <summary>
    ///     Объект удовлетворяет заданному регулярному выражению.
    /// </summary>
    public sealed class RegexValidator : BaseValidationOperator
    {
        public string Pattern { get; set; }

        protected override bool ValidateObject(object validationObject)
        {
            var targetAsString = validationObject as string;
            return (targetAsString != null) && Regex.IsMatch(targetAsString, Pattern);
        }
    }
}