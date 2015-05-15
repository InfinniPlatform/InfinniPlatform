using InfinniPlatform.Api.Validation.BooleanValidators;
using InfinniPlatform.Api.Validation.CollectionValidators;
using InfinniPlatform.Api.Validation.ObjectValidators;

namespace InfinniPlatform.Api.Validation.ValidationBuilders
{
	public static class CollectionValidationBuilderExtensions
	{
		// IsNull

		public static CollectionValidationBuilder IsNull(this CollectionValidationBuilder target, object message)
		{
			return target.Predicate(new NullValidator { Message = message });
		}

		public static CollectionValidationGenericBuilder<T> IsNull<T>(this CollectionValidationGenericBuilder<T> target, object message)
		{
			return target.Predicate(new NullValidator { Message = message });
		}

		public static CollectionValidationBuilder IsNotNull(this CollectionValidationBuilder target, object message)
		{
			return target.Predicate(new NotValidator { Operator = new NullValidator(), Message = message });
		}

		public static CollectionValidationGenericBuilder<T> IsNotNull<T>(this CollectionValidationGenericBuilder<T> target, object message)
		{
			return target.Predicate(new NotValidator { Operator = new NullValidator(), Message = message });
		}


		// IsNullOrEmpty

		public static CollectionValidationBuilder IsNullOrEmpty(this CollectionValidationBuilder target, object message)
		{
			return target.Predicate(new NullOrEmptyCollectionValidator { Message = message });
		}

		public static CollectionValidationGenericBuilder<T> IsNullOrEmpty<T>(this CollectionValidationGenericBuilder<T> target, object message)
		{
			return target.Predicate(new NullOrEmptyCollectionValidator { Message = message });
		}

		public static CollectionValidationBuilder IsNotNullOrEmpty(this CollectionValidationBuilder target, object message)
		{
			return target.Predicate(new NotValidator { Operator = new NullOrEmptyCollectionValidator(), Message = message });
		}

		public static CollectionValidationGenericBuilder<T> IsNotNullOrEmpty<T>(this CollectionValidationGenericBuilder<T> target, object message)
		{
			return target.Predicate(new NotValidator { Operator = new NullOrEmptyCollectionValidator(), Message = message });
		}


		// IsContains

		public static CollectionValidationBuilder IsContains(this CollectionValidationBuilder target, object value, object message)
		{
			return target.Predicate(new ContainsCollectionValidator { Message = message, Value = value });
		}

		public static CollectionValidationGenericBuilder<T> IsContains<T>(this CollectionValidationGenericBuilder<T> target, T value, object message)
		{
			return target.Predicate(new ContainsCollectionValidator { Message = message, Value = value });
		}

		public static CollectionValidationBuilder IsNotContains(this CollectionValidationBuilder target, object value, object message)
		{
			return target.Predicate(new NotValidator { Operator = new ContainsCollectionValidator { Value = value }, Message = message });
		}

		public static CollectionValidationGenericBuilder<T> IsNotContains<T>(this CollectionValidationGenericBuilder<T> target, T value, object message)
		{
			return target.Predicate(new NotValidator { Operator = new ContainsCollectionValidator { Value = value }, Message = message });
		}
	}
}