using InfinniPlatform.Core.Validation;

namespace InfinniPlatform.Core.Tests.Validation
{
    internal sealed class FaseValidationOperator : BaseValidationOperator
    {
        protected override bool ValidateObject(object validationObject)
        {
            return false;
        }
    }
}