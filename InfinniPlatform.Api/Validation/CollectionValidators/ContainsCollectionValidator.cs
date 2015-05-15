using System.Linq;

namespace InfinniPlatform.Api.Validation.CollectionValidators
{
	/// <summary>
	/// Коллекция содержит заданное значение.
	/// </summary>
	public sealed class ContainsCollectionValidator : BaseValidationOperator
	{
		public object Value { get; set; }

		protected override bool ValidateObject(object validationObject)
		{
			var collection = validationObject.TryCastToEnumerable();
			return (collection != null) && collection.Contains(Value);
		}
	}
}