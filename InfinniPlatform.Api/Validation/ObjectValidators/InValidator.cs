using System.Collections;
using System.Linq;

namespace InfinniPlatform.Api.Validation.ObjectValidators
{
    /// <summary>
    ///     Объект содержится в заданной коллекции.
    /// </summary>
    public sealed class InValidator : BaseValidationOperator
    {
        public IEnumerable Items { get; set; }

        protected override bool ValidateObject(object validationObject)
        {
            return (Items != null) && Items.Cast<object>().Contains(validationObject);
        }
    }
}