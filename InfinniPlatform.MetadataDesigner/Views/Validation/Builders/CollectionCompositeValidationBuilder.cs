using System;

using InfinniPlatform.Core.Validation;
using InfinniPlatform.Core.Validation.BooleanValidators;
using InfinniPlatform.Sdk.Environment;

namespace InfinniPlatform.MetadataDesigner.Views.Validation.Builders
{
	public sealed class CollectionCompositeValidationBuilder
	{
		public CollectionCompositeValidationBuilder(string property = null)
		{
			Property = property;
		}


		public string Property { get; private set; }

		public IValidationOperator Operator { get; private set; }


		/// <summary>
		/// Добавить правило логического сложения для текущей коллекции.
		/// </summary>
		public void Or(Action<CollectionValidationBuilder> buildAction)
		{
			var validationOperator = new OrValidator { Property = Property };
			buildAction(new CollectionValidationBuilder(validationOperator));

			Operator = validationOperator;
		}

		/// <summary>
		/// Добавить правило логического умножения для текущей коллекции.
		/// </summary>
		public void And(Action<CollectionValidationBuilder> buildAction)
		{
			var validationOperator = new AndValidator { Property = Property };
			buildAction(new CollectionValidationBuilder(validationOperator));

			Operator = validationOperator;
		}
	}
}