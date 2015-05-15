﻿using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Serialization;
using InfinniPlatform.Api.Validation.BooleanValidators;
using InfinniPlatform.Api.Validation.CollectionValidators;
using InfinniPlatform.Api.Validation.ObjectValidators;

namespace InfinniPlatform.Api.Validation.Serialization
{
	/// <summary>
	/// Предоставляет методы для сериализации и десериализации операторов валидации.
	/// </summary>
	public sealed class ValidationOperatorSerializer
	{
		public static readonly ValidationOperatorSerializer Instance
			= new ValidationOperatorSerializer();


		public ValidationOperatorSerializer()
		{
			_serializer = new JsonObjectSerializer(
				new KnownTypesContainer()
				// BooleanValidators
					.Add<OrValidator>("Or")
					.Add<AndValidator>("And")
					.Add<NotValidator>("Not")
				// CollectionValidators
					.Add<AllCollectionValidator>("All")
					.Add<AnyCollectionValidator>("Any")
					.Add<ContainsCollectionValidator>("IsContainsCollection")
					.Add<NullOrEmptyCollectionValidator>("IsNullOrEmptyCollection")
				// ObjectValidators
					.Add<NullValidator>("IsNull")
					.Add<EqualValidator>("IsEqual")
					.Add<DefaultValueValidator>("IsDefaultValue")
					.Add<GuidValidator>("IsGuid")
					.Add<UriValidator>("IsUri")
					.Add<AbsoluteUriValidator>("IsAbsoluteUri")
					.Add<RelativeUriValidator>("IsRelativeUri")
					.Add<NullOrEmptyValidator>("IsNullOrEmpty")
					.Add<NullOrWhiteSpaceValidator>("IsNullOrWhiteSpace")
					.Add<ContainsValidator>("IsContains")
					.Add<StartsWithValidator>("IsStartsWith")
					.Add<EndsWithValidator>("IsEndsWith")
					.Add<RegexValidator>("IsRegex")
					.Add<LessThanValidator>("IsLessThan")
					.Add<MoreThanValidator>("IsMoreThan")
					.Add<LessThanOrEqualValidator>("IsLessThanOrEqual")
					.Add<MoreThanOrEqualValidator>("IsMoreThanOrEqual")
					.Add<InValidator>("IsIn")
				);
		}


		private readonly JsonObjectSerializer _serializer;


		public dynamic Serialize(IValidationOperator value)
		{
			return _serializer.ConvertToDynamic(value);
		}

		public IValidationOperator Deserialize(dynamic value)
		{
			dynamic dynamicWrapper = new DynamicWrapper();
			dynamicWrapper.Operator = value;

			// Проблема в том, что при десериализации нужно указать конкретный тип, поэтому понадобилась обертка
			var wrapper = (ValidationOperatorWrapper)_serializer.ConvertFromDynamic(dynamicWrapper, typeof(ValidationOperatorWrapper));

			return wrapper.Operator;
		}


		class ValidationOperatorWrapper
		{
			public IValidationOperator Operator { get; set; }
		}
	}
}