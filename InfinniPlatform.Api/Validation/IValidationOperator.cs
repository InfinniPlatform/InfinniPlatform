namespace InfinniPlatform.Api.Validation
{
	/// <summary>
	/// Оператор для проверки объекта.
	/// </summary>
	public interface IValidationOperator
	{
		/// <summary>
		/// Проверяет объект.
		/// </summary>
		/// <param name="validationObject">Проверяемый объект.</param>
		/// <param name="validationResult">Результат проверки объекта.</param>
		/// <param name="parentProperty">Путь проверяемому объекту.</param>
		bool Validate(object validationObject, ValidationResult validationResult = null, string parentProperty = null);
	}
}