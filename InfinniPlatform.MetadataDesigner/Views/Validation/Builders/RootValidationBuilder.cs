using System;
using InfinniPlatform.Api.Validation;

namespace InfinniPlatform.MetadataDesigner.Views.Validation.Builders
{
	public static class RootValidationBuilder
	{
		/// <summary>
		/// Построить правила проверки для объекта.
		/// </summary>
		public static IValidationOperator ForObject(Action<ObjectCompositeValidationBuilder> buildAction)
		{
			var builder = new ObjectCompositeValidationBuilder();
			buildAction(builder);

			return builder.Operator;
		}

		/// <summary>
		/// Построить правила проверки для коллекции.
		/// </summary>
		public static IValidationOperator ForCollection(Action<CollectionCompositeValidationBuilder> buildAction)
		{
			var builder = new CollectionCompositeValidationBuilder();
			buildAction(builder);

			return builder.Operator;
		}
	}
}