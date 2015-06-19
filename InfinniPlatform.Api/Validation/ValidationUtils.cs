using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Api.Validation
{
    public static class ValidationUtils
    {
        public static void CreateValidationMessage(this IApplyContext target, string message, bool isErrorMessage)
        {
            target.ValidationMessage = new DynamicWrapper();
            if (isErrorMessage)
            {
                target.ValidationMessage.ValidationErrors = new DynamicWrapper();
                target.ValidationMessage.ValidationErrors.Message = message;
                target.ValidationMessage.ValidationErrors.IsValid = false;
            }
            else
            {
                target.ValidationMessage.ValidationWarnings = new DynamicWrapper();
                target.ValidationMessage.ValidationWarnings.Message = message;
                target.ValidationMessage.ValidationWarnings.IsValid = false;
            }
            target.IsValid = false;
        }
    }
}