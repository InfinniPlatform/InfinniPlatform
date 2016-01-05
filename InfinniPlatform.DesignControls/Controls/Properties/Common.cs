using System;
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

            validationResult.Items.Add(string.Format("PropertyShouldntBeEmpty", propertyName, objectValidateName));
            return validationResult;
        }

        public static Func<Func<string, object>, ValidationResult> CreateNullOrEmptyValidator(string objectName,
            string propertyName)
        {
            return func => NullOrEmptyValidator(func, objectName, propertyName);
        }
    }
}