using InfinniPlatform.Core.Serialization;
using InfinniPlatform.Core.Validation.BooleanValidators;
using InfinniPlatform.Core.Validation.CollectionValidators;
using InfinniPlatform.Core.Validation.ObjectValidators;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Validation.Serialization
{
    /// <summary>
    ///     Предоставляет методы для сериализации и десериализации операторов валидации.
    /// </summary>
    public sealed class ValidationOperatorSerializer
    {
        public static readonly ValidationOperatorSerializer Instance
            = new ValidationOperatorSerializer();

        private readonly JsonObjectSerializer _serializer;

        public ValidationOperatorSerializer()
        {
            _serializer = new JsonObjectSerializer(false,
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

        public dynamic Serialize(IValidationOperator value)
        {
            return _serializer.ConvertToDynamic(value);
        }

        public IValidationOperator Deserialize(dynamic value)
        {
            dynamic dynamicWrapper = new DynamicWrapper();
            dynamicWrapper.Operator = value;

            // Проблема в том, что при десериализации нужно указать конкретный тип, поэтому понадобилась обертка
            var wrapper =
                (ValidationOperatorWrapper)
                    _serializer.ConvertFromDynamic(dynamicWrapper, typeof (ValidationOperatorWrapper));

            return wrapper.Operator;
        }

        private class ValidationOperatorWrapper
        {
            public IValidationOperator Operator { get; set; }
        }
    }
}