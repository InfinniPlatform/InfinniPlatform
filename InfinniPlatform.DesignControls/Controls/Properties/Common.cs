using System;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;

namespace InfinniPlatform.DesignControls.Controls.Properties
{
    public static class Common
    {
        public static ValidationResult NullOrEmptyValidator(Func<string, dynamic> getItemPropertyFunc,
            string objectValidateName, string propertyName)
        {
            var validationResult = new ValidationResult();

            var item = getItemPropertyFunc(propertyName);
            if (item != null)
            {
                validationResult.IsValid = !string.IsNullOrEmpty(item.ToString());
            }
            else
            {
                validationResult.IsValid = false;
            }

            validationResult.Items.Add(string.Format(Resources.PropertyShouldntBeEmpty, propertyName, objectValidateName));
            return validationResult;
        }

        public static Func<Func<string, object>, ValidationResult> CreateNullOrEmptyValidator(string objectName,
            string propertyName)
        {
            return func => NullOrEmptyValidator(func, objectName, propertyName);
        }
    }
}