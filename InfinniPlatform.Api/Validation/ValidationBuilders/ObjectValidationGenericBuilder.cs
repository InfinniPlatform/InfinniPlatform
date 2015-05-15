using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using InfinniPlatform.Api.Validation.BooleanValidators;

namespace InfinniPlatform.Api.Validation.ValidationBuilders
{
	public sealed class ObjectValidationGenericBuilder<T>
	{
		public ObjectValidationGenericBuilder(CompositeValidator compositeValidator)
		{
			_compositeValidator = compositeValidator;
		}


		private readonly CompositeValidator _compositeValidator;


		/// <summary>
		/// Добавить правило проверки текущего объекта.
		/// </summary>
		public ObjectValidationGenericBuilder<T> Predicate(IValidationOperator predicate)
		{
			_compositeValidator.Add(predicate);

			return this;
		}


		/// <summary>
		/// Добавить правило проверки свойства текущего объекта.
		/// </summary>
		public ObjectValidationGenericBuilder<T> Property<TProperty>(Expression<Func<T, TProperty>> property, Action<ObjectCompositeValidationGenericBuilder<TProperty>> buildAction)
		{
			var builder = new ObjectCompositeValidationGenericBuilder<TProperty>(Reflection.GetPropertyPath(property));

			buildAction(builder);

			_compositeValidator.Add(builder.Operator);

			return this;
		}

		/// <summary>
		/// Добавить правило проверки коллекции текущего объекта.
		/// </summary>
		public ObjectValidationGenericBuilder<T> Collection<TItem>(Expression<Func<T, IEnumerable<TItem>>> collection, Action<CollectionCompositeValidationGenericBuilder<TItem>> buildAction)
		{
			var builder = new CollectionCompositeValidationGenericBuilder<TItem>(Reflection.GetCollectionPath(collection));

			buildAction(builder);

			_compositeValidator.Add(builder.Operator);

			return this;
		}


		/// <summary>
		/// Добавить правило логического сложения для текущего объекта.
		/// </summary>
		public ObjectValidationGenericBuilder<T> Or(Action<ObjectValidationGenericBuilder<T>> buildAction)
		{
			var compositeValidationBuilder = new ObjectCompositeValidationGenericBuilder<T>();
			compositeValidationBuilder.Or(buildAction);

			_compositeValidator.Add(compositeValidationBuilder.Operator);

			return this;
		}

		/// <summary>
		/// Добавить правило логического умножения для текущего объекта.
		/// </summary>
		public ObjectValidationGenericBuilder<T> And(Action<ObjectValidationGenericBuilder<T>> buildAction)
		{
			var compositeValidationBuilder = new ObjectCompositeValidationGenericBuilder<T>();
			compositeValidationBuilder.And(buildAction);

			_compositeValidator.Add(compositeValidationBuilder.Operator);

			return this;
		}
	}
}