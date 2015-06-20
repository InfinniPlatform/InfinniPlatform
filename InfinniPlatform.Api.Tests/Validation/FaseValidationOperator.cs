using InfinniPlatform.Api.Validation;

namespace InfinniPlatform.Api.Tests.Validation
{
    internal sealed class FaseValidationOperator : BaseValidationOperator
    {
        protected override bool ValidateObject(object validationObject)
        {
            return false;
        }
    }
}