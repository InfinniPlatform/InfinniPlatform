using System;

namespace InfinniPlatform.Api.Validation.ObjectValidators
{
	/// <summary>
	/// Объект начинается заданной подстрокой.
	/// </summary>
	public class StartsWithValidator : BaseValidationOperator
	{
		public string Value { get; set; }

		protected override bool ValidateObject(object validationObject)
		{
			var targetAsString = validationObject as string;
			return (targetAsString != null) && targetAsString.StartsWith(Value, StringComparison.OrdinalIgnoreCase);
		}
	}
}