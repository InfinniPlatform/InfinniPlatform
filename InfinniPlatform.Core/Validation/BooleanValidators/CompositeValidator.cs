using System.Collections.Generic;

namespace InfinniPlatform.Core.Validation.BooleanValidators
{
    /// <summary>
    ///     Базовый класс композитных операторов для проверки объекта.
    /// </summary>
    public abstract class CompositeValidator : IValidationOperator
    {
        public string Property { get; set; }
        public ICollection<IValidationOperator> Operators { get; set; }

        public abstract bool Validate(object validationObject, ValidationResult validationResult = null,
            string parentProperty = null);

        public void Add(IValidationOperator validationOperator)
        {
            if (Operators == null)
            {
                Operators = new List<IValidationOperator>();
            }

            Operators.Add(validationOperator);
        }

        public void Remove(IValidationOperator validationOperator)
        {
            if (Operators != null)
            {
                Operators.Remove(validationOperator);
            }
        }
    }
}