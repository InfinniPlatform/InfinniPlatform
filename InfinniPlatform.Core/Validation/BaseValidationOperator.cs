using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Validation
{
    /// <summary>
    ///     Базовый класс оператора для проверки объекта.
    /// </summary>
    public abstract class BaseValidationOperator : IValidationOperator
    {
        /// <summary>
        ///     Возвращает или устанавливает путь к свойству проверяемого объекта, которое следует проверить.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает сообщение об ошибке для случая, если свойство проверяемого объекта не прошло проверку.
        /// </summary>
        public object Message { get; set; }

        public virtual bool Validate(object validationObject, ValidationResult validationResult = null,
            string parentProperty = null)
        {
            var propertyValue = validationObject.GetProperty(Property);

            var isValid = ValidateObject(propertyValue);

            validationResult.SetValidationResult(isValid, parentProperty, Property, Message);

            return isValid;
        }

        /// <summary>
        ///     Проверяет, удовлетворяет ли свойство проверяемого объекта условию оператора.
        /// </summary>
        protected abstract bool ValidateObject(object validationObject);
    }
}