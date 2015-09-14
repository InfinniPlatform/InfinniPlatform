using InfinniPlatform.Api.Validation;

namespace InfinniPlatform.Api.Tests.Validation
{
    internal sealed class TrueValidationOperator : BaseValidationOperator
    {
        protected override bool ValidateObject(object validationObject)
        {
            return true;
        }
    }
}