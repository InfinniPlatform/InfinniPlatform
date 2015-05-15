using System;

namespace InfinniPlatform.Api.Validation.ObjectValidators
{
	/// <summary>
	/// Объект является относительным URI.
	/// </summary>
	public sealed class RelativeUriValidator : BaseValidationOperator
	{
		protected override bool ValidateObject(object validationObject)
		{
			Uri uri;
			return (validationObject != null) && Uri.TryCreate(validationObject.ToString(), UriKind.Relative, out uri);
		}
	}
}