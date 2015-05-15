using System;

using InfinniPlatform.Api.Validation.BooleanValidators;

namespace InfinniPlatform.Api.Validation.ValidationBuilders
{
	public sealed class ObjectCompositeValidationGenericBuilder<T>
	{
		public ObjectCompositeValidationGenericBuilder(string property = null)
		{
			Property = property;
		}


		public string Property { get; private set; }

		public IValidationOperator Operator { get; private set; }


		/// <summary>
		/// Добавить правило логического сложения для текущего объекта.
		/// </summary>
		public void Or(Action<ObjectValidationGenericBuilder<T>> buildAction)
		{
			var validationOperator = new OrValidator { Property = Property };
			buildAction(new ObjectValidationGenericBuilder<T>(validationOperator));

			Operator = validationOperator;
		}

		/// <summary>
		/// Добавить правило логического умножения для текущего объекта.
		/// </summary>
		public void And(Action<ObjectValidationGenericBuilder<T>> buildAction)
		{
			var validationOperator = new AndValidator { Property = Property };
			buildAction(new ObjectValidationGenericBuilder<T>(validationOperator));

			Operator = validationOperator;
		}
	}
}