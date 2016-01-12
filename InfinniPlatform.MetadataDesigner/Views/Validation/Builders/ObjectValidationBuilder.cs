using System.Collections;
using System;

using InfinniPlatform.Core.Validation;
using InfinniPlatform.Core.Validation.BooleanValidators;
using InfinniPlatform.Core.Validation.ObjectValidators;
using InfinniPlatform.Sdk.Environment;

namespace InfinniPlatform.MetadataDesigner.Views.Validation.Builders
{
	public sealed class ObjectValidationBuilder
	{
		public ObjectValidationBuilder(CompositeValidator compositeValidator)
		{
			_compositeValidator = compositeValidator;
		}


		private readonly CompositeValidator _compositeValidator;


		/// <summary>
		/// Добавить правило проверки текущего объекта.
		/// </summary>
		public ObjectValidationBuilder Predicate(IValidationOperator predicate)
		{
			_compositeValidator.Add(predicate);

			return this;
		}


		/// <summary>
		/// Добавить правило проверки свойства текущего объекта.
		/// </summary>
		public ObjectValidationBuilder Property(string property, Action<ObjectCompositeValidationBuilder> buildAction)
		{
			var builder = new ObjectCompositeValidationBuilder(property);

			buildAction(builder);

			_compositeValidator.Add(builder.Operator);

			return this;
		}

		/// <summary>
		/// Добавить правило проверки коллекции текущего объекта.
		/// </summary>
		public ObjectValidationBuilder Collection(string property, Action<CollectionCompositeValidationBuilder> buildAction)
		{
			var builder = new CollectionCompositeValidationBuilder(property);

			buildAction(builder);

			_compositeValidator.Add(builder.Operator);

			return this;
		}


		/// <summary>
		/// Добавить правило логического сложения для текущего объекта.
		/// </summary>
		public ObjectValidationBuilder Or(Action<ObjectValidationBuilder> buildAction)
		{
			var compositeValidationBuilder = new ObjectCompositeValidationBuilder();
			compositeValidationBuilder.Or(buildAction);

			_compositeValidator.Add(compositeValidationBuilder.Operator);

			return this;
		}

		/// <summary>
		/// Добавить правило логического умножения для текущего объекта.
		/// </summary>
		public ObjectValidationBuilder And(Action<ObjectValidationBuilder> buildAction)
		{
			var compositeValidationBuilder = new ObjectCompositeValidationBuilder();
			compositeValidationBuilder.And(buildAction);

			_compositeValidator.Add(compositeValidationBuilder.Operator);

			return this;
		}

        // IsNull

        public static ObjectValidationBuilder IsNull(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new NullValidator { Message = message });
        }

        public static ObjectValidationBuilder IsNull(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new NullValidator { Property = property, Message = message });
        }

        public static ObjectValidationBuilder IsNotNull(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new NotValidator { Operator = new NullValidator(), Message = message });
        }

        public static ObjectValidationBuilder IsNotNull(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new NotValidator { Property = property, Operator = new NullValidator() , Message = message });
        }

        // IsDefault

        public static ObjectValidationBuilder IsDefault(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new DefaultValueValidator { Message = message });
        }

        public static ObjectValidationBuilder IsDefault(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new DefaultValueValidator { Property = property, Message = message });
        }

        public static ObjectValidationBuilder IsNotDefault(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new NotValidator { Operator = new DefaultValueValidator(), Message = message });
        }

        public static ObjectValidationBuilder IsNotDefault(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new NotValidator { Property = property, Operator = new DefaultValueValidator(), Message = message });
        }

        // IsEqual

        public static ObjectValidationBuilder IsEqual(ObjectValidationBuilder target, object value, string message)
        {
            return target.Predicate(new EqualValidator { Value = value, Message = message });
        }

        public static ObjectValidationBuilder IsEqual(ObjectValidationBuilder target, string property, object value, string message)
        {
            return target.Predicate(new EqualValidator { Property = property, Value = value, Message = message });
        }

        public static ObjectValidationBuilder IsNotEqual(ObjectValidationBuilder target, object value, string message)
        {
            return target.Predicate(new NotValidator { Operator = new EqualValidator { Value = value }, Message = message });
        }

        public static ObjectValidationBuilder IsNotEqual(ObjectValidationBuilder target, string property, object value, string message)
        {
            return target.Predicate(new NotValidator { Property = property, Operator = new EqualValidator { Value = value }, Message = message });
        }

        // IsIn

        public static ObjectValidationBuilder IsIn(ObjectValidationBuilder target, IEnumerable items, string message)
        {
            return target.Predicate(new InValidator { Items = items, Message = message });
        }

        public static ObjectValidationBuilder IsIn(ObjectValidationBuilder target, string property, IEnumerable items, string message)
        {
            return target.Predicate(new InValidator { Property = property, Items = items, Message = message });
        }

        public static ObjectValidationBuilder IsNotIn(ObjectValidationBuilder target, IEnumerable items, string message)
        {
            return target.Predicate(new NotValidator { Operator = new InValidator { Items = items }, Message = message });
        }

        public static ObjectValidationBuilder IsNotIn(ObjectValidationBuilder target, string property, IEnumerable items, string message)
        {
            return target.Predicate(new NotValidator { Property = property, Operator = new InValidator { Items = items }, Message = message });
        }

        // IsNullOrEmpty

        public static ObjectValidationBuilder IsNullOrEmpty(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new NullOrEmptyValidator { Message = message });
        }

        public static ObjectValidationBuilder IsNullOrEmpty(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new NullOrEmptyValidator { Property = property, Message = message });
        }

        public static ObjectValidationBuilder IsNotNullOrEmpty(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new NotValidator { Operator = new NullOrEmptyValidator(), Message = message });
        }

        public static ObjectValidationBuilder IsNotNullOrEmpty(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new NotValidator { Property =  property, Operator = new NullOrEmptyValidator (), Message = message });
        }

        // IsNullOrWhiteSpace

        public static ObjectValidationBuilder IsNullOrWhiteSpace(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new NullOrWhiteSpaceValidator { Message = message });
        }

        public static ObjectValidationBuilder IsNullOrWhiteSpace(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new NullOrWhiteSpaceValidator { Property = property, Message = message });
        }

        public static ObjectValidationBuilder IsNotNullOrWhiteSpace(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new NotValidator { Operator = new NullOrWhiteSpaceValidator(), Message = message });
        }

        public static ObjectValidationBuilder IsNotNullOrWhiteSpace(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new NotValidator {Property = property, Operator = new NullOrWhiteSpaceValidator(), Message = message });
        }

        // IsStartsWith

        public static ObjectValidationBuilder IsStartsWith(ObjectValidationBuilder target, string value, string message)
        {
            return target.Predicate(new StartsWithValidator { Value = value, Message = message });
        }

        public static ObjectValidationBuilder IsStartsWith(ObjectValidationBuilder target, string property, string value, string message)
        {
            return target.Predicate(new StartsWithValidator { Property = property, Value = value, Message = message });
        }

        public static ObjectValidationBuilder IsNotStartsWith(ObjectValidationBuilder target, string value, string message)
        {
            return target.Predicate(new NotValidator { Operator = new StartsWithValidator { Value = value }, Message = message });
        }

        public static ObjectValidationBuilder IsNotStartsWith(ObjectValidationBuilder target, string property, string value, string message)
        {
            return target.Predicate(new NotValidator { Property = property, Operator = new StartsWithValidator {Value = value}, Message = message });
        }

        // IsEndsWith

        public static ObjectValidationBuilder IsEndsWith(ObjectValidationBuilder target, string value, string message)
        {
            return target.Predicate(new EndsWithValidator { Value = value, Message = message });
        }

        public static ObjectValidationBuilder IsEndsWith(ObjectValidationBuilder target, string property, string value, string message)
        {
            return target.Predicate(new EndsWithValidator { Property = property, Value = value, Message = message });
        }

        public static ObjectValidationBuilder IsNotEndsWith(ObjectValidationBuilder target, string value, string message)
        {
            return target.Predicate(new NotValidator { Operator = new EndsWithValidator { Value = value }, Message = message });
        }

        public static ObjectValidationBuilder IsNotEndsWith(ObjectValidationBuilder target, string property, string value, string message)
        {
            return target.Predicate(new NotValidator {Property = property, Operator = new EndsWithValidator { Value = value }, Message = message });
        }

        // IsContains

        public static ObjectValidationBuilder IsContains(ObjectValidationBuilder target, string value, string message)
        {
            return target.Predicate(new ContainsValidator { Value = value, Message = message });
        }

        public static ObjectValidationBuilder IsContains(ObjectValidationBuilder target, string property, string value, string message)
        {
            return target.Predicate(new ContainsValidator { Property = property, Value = value, Message = message });
        }

        public static ObjectValidationBuilder IsNotContains(ObjectValidationBuilder target, string value, string message)
        {
            return target.Predicate(new NotValidator { Operator = new ContainsValidator { Value = value }, Message = message });
        }

        public static ObjectValidationBuilder IsNotContains(ObjectValidationBuilder target, string property, string value, string message)
        {
            return target.Predicate(new NotValidator { Property = property, Operator = new ContainsValidator { Value = value }, Message = message });
        }


        // IsRegex

        public static ObjectValidationBuilder IsRegex(ObjectValidationBuilder target, string pattern, string message)
        {
            return target.Predicate(new RegexValidator { Pattern = pattern, Message = message });
        }

        public static ObjectValidationBuilder IsRegex(ObjectValidationBuilder target, string property, string pattern, string message)
        {
            return target.Predicate(new RegexValidator { Property = property, Pattern = pattern, Message = message });
        }

        public static ObjectValidationBuilder IsNotRegex(ObjectValidationBuilder target, string pattern, string message)
        {
            return target.Predicate(new NotValidator { Operator = new RegexValidator { Pattern = pattern }, Message = message });
        }

        public static ObjectValidationBuilder IsNotRegex(ObjectValidationBuilder target, string property, string pattern, string message)
        {
            return target.Predicate(new NotValidator {Property = property, Operator = new RegexValidator { Pattern = pattern }, Message = message });
        }

        // IsGuid

        public static ObjectValidationBuilder IsGuid(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new GuidValidator { Message = message });
        }

        public static ObjectValidationBuilder IsGuid(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new GuidValidator { Property = property, Message = message });
        }

        public static ObjectValidationBuilder IsNotGuid(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new NotValidator { Operator = new GuidValidator(), Message = message });
        }

        public static ObjectValidationBuilder IsNotGuid(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new NotValidator {Property = property, Operator = new GuidValidator (), Message = message });
        }

        // IsAbsoluteUri

        public static ObjectValidationBuilder IsAbsoluteUri(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new AbsoluteUriValidator { Message = message });
        }

        public static ObjectValidationBuilder IsAbsoluteUri(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new AbsoluteUriValidator { Property = property, Message = message });
        }

        public static ObjectValidationBuilder IsNotAbsoluteUri(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new NotValidator { Operator = new AbsoluteUriValidator(), Message = message });
        }

        public static ObjectValidationBuilder IsNotAbsoluteUri(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new NotValidator {Property = property, Operator = new AbsoluteUriValidator (), Message = message });
        }

        // IsRelativeUri

        public static ObjectValidationBuilder IsRelativeUri(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new RelativeUriValidator { Message = message });
        }

        public static ObjectValidationBuilder IsRelativeUri(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new RelativeUriValidator { Property = property, Message = message });
        }

        public static ObjectValidationBuilder IsNotRelativeUri(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new NotValidator { Operator = new RelativeUriValidator(), Message = message });
        }

        public static ObjectValidationBuilder IsNotRelativeUri(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new NotValidator {Property = property, Operator = new RelativeUriValidator (), Message = message });
        }

        // IsUri

        public static ObjectValidationBuilder IsUri(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new UriValidator { Message = message });
        }

        public static ObjectValidationBuilder IsUri(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new UriValidator { Property = property, Message = message });
        }

        public static ObjectValidationBuilder IsNotUri(ObjectValidationBuilder target, string message)
        {
            return target.Predicate(new NotValidator { Operator = new UriValidator(), Message = message });
        }

        public static ObjectValidationBuilder IsNotUri(ObjectValidationBuilder target, string property, string message)
        {
            return target.Predicate(new NotValidator {Property = property, Operator = new UriValidator() , Message = message });
        }
	}
}