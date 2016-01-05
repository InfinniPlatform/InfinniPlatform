using System;

using InfinniPlatform.Core.Validation.BooleanValidators;
using InfinniPlatform.Core.Validation.CollectionValidators;
using InfinniPlatform.Core.Validation.ObjectValidators;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.MetadataDesigner.Views.Validation.Builders
{
	public sealed class CollectionValidationBuilder
	{
		public CollectionValidationBuilder(CompositeValidator compositeValidator)
		{
			_compositeValidator = compositeValidator;
		}


		private readonly CompositeValidator _compositeValidator;


		/// <summary>
		/// Добавить правило проверки текущей коллекции.
		/// </summary>
		public CollectionValidationBuilder Predicate(IValidationOperator predicate)
		{
			_compositeValidator.Add(predicate);

			return this;
		}

		/// <summary>
		/// Добавить правило проверки текущей коллекции, что все ее элементы удолетворяет условиям.
		/// </summary>
		public CollectionValidationBuilder All(Action<ObjectCompositeValidationBuilder> buildAction)
		{
			var itemBuilder = new ObjectCompositeValidationBuilder();
			buildAction(itemBuilder);

			_compositeValidator.Add(new AllCollectionValidator { Operator = itemBuilder.Operator });

			return this;
		}

		/// <summary>
		/// Добавить правило проверки текущей коллекции, что хотя бы один из ее элементов удолетворяет условиям.
		/// </summary>
		public CollectionValidationBuilder Any(Action<ObjectCompositeValidationBuilder> buildAction)
		{
			var itemBuilder = new ObjectCompositeValidationBuilder();
			buildAction(itemBuilder);

			_compositeValidator.Add(new AnyCollectionValidator { Operator = itemBuilder.Operator });

			return this;
		}


		/// <summary>
		/// Добавить правило логического сложения для текущей коллекции.
		/// </summary>
		public CollectionValidationBuilder Or(Action<CollectionValidationBuilder> buildAction)
		{
			var compositeValidationBuilder = new CollectionCompositeValidationBuilder();
			compositeValidationBuilder.Or(buildAction);

			_compositeValidator.Add(compositeValidationBuilder.Operator);

			return this;
		}

		/// <summary>
		/// Добавить правило логического умножения для текущей коллекции.
		/// </summary>
		public CollectionValidationBuilder And(Action<CollectionValidationBuilder> buildAction)
		{
			var compositeValidationBuilder = new CollectionCompositeValidationBuilder();
			compositeValidationBuilder.And(buildAction);

			_compositeValidator.Add(compositeValidationBuilder.Operator);

			return this;
		}

        // IsNull

        public static CollectionValidationBuilder IsNull(CollectionValidationBuilder target, string message)
        {
            return target.Predicate(new NullValidator { Message = message });
        }

        public static CollectionValidationBuilder IsNotNull(CollectionValidationBuilder target, string message)
        {
            return target.Predicate(new NotValidator { Operator = new NullValidator(), Message = message });
        }

        // IsNullOrEmpty

        public static CollectionValidationBuilder IsNullOrEmptyCollection(CollectionValidationBuilder target, string message)
        {
            return target.Predicate(new NullOrEmptyCollectionValidator { Message = message });
        }

        public static CollectionValidationBuilder IsNotNullOrEmptyCollection(CollectionValidationBuilder target, string message)
        {
            return target.Predicate(new NotValidator { Operator = new NullOrEmptyCollectionValidator(), Message = message });
        }

        // IsContains

        public static CollectionValidationBuilder IsContainsCollection(CollectionValidationBuilder target, object value, string message)
        {
            return target.Predicate(new ContainsCollectionValidator { Message = message, Value = value });
        }

        public static CollectionValidationBuilder IsNotContainsCollection(CollectionValidationBuilder target, object value, string message)
        {
            return target.Predicate(new NotValidator { Operator = new ContainsCollectionValidator { Value = value }, Message = message });
        }
	}
}