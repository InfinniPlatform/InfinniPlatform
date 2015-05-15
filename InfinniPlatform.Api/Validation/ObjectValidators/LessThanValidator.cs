using System;

namespace InfinniPlatform.Api.Validation.ObjectValidators
{
	/// <summary>
	/// Объект меньше заданного объекта.
	/// </summary>
	public sealed class LessThanValidator : BaseValidationOperator
	{
		public object Value { get; set; }

		protected override bool ValidateObject(object validationObject)
		{
			return (validationObject != null) && (dynamic)validationObject < (dynamic)Convert.ChangeType(Value, validationObject.GetType());
		}
	}
}