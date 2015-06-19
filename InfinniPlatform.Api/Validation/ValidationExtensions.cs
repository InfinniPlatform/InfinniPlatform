using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.Validation.Serialization;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Api.Validation
{
    public static class ValidationExtensions
    {
        public static string CombineProperties(string parent, string property)
        {
            var parentIsNull = string.IsNullOrEmpty(parent);
            var propertyIsNull = string.IsNullOrEmpty(property);

            var result = parent;

            if (!parentIsNull && !propertyIsNull)
            {
                result += '.' + property;
            }
            else if (parentIsNull)
            {
                result = property;
            }

            return result ?? string.Empty;
        }

        public static IEnumerable<object> TryCastToEnumerable(this object target)
        {
            if (target is DynamicWrapper)
            {
                return target.ToEnumerable();
            }

            if (target is IEnumerable)
            {
                return ((IEnumerable) target).OfType<object>().ToArray();
            }

            return null;
        }

        public static void SetValidationResult(this ValidationResult result, bool isValid, string parent,
            string property, object message)
        {
            if (result != null)
            {
                result.IsValid = isValid;

                if (!isValid)
                {
                    if (result.Items == null)
                    {
                        result.Items = new List<dynamic>();
                    }

                    dynamic validationResultItem = new DynamicWrapper();
                    validationResultItem.Property = CombineProperties(parent, property);
                    validationResultItem.Message = message;

                    result.Items.Add(validationResultItem);
                }
            }
        }

        public static void SetValidationResult(this ValidationResult result, bool isValid, ValidationResult source)
        {
            if (result != null)
            {
                result.IsValid = isValid;

                if (!isValid && source != null && source.Items.Count > 0)
                {
                    if (result.Items == null)
                    {
                        result.Items = new List<dynamic>();
                    }

                    result.Items.AddRange(source.Items);
                }
            }
        }

        public static IValidationOperator CreateValidatorFromConfigValidator(dynamic validatorConfig)
        {
            if (validatorConfig == null)
            {
                throw new ArgumentException(Resources.ValidatorNotFound);
            }

            if (validatorConfig is string)
            {
                return ValidationOperatorSerializer.Instance.Deserialize(((object) validatorConfig).ToDynamic());
            }
            return ValidationOperatorSerializer.Instance.Deserialize(validatorConfig);
        }
    }
}