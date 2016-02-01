using InfinniPlatform.Core.Validation;

namespace InfinniPlatform.Core.Tests.Validation
{
    internal sealed class TrueValidationOperator : BaseValidationOperator
    {
        protected override bool ValidateObject(object validationObject)
        {
            return true;
        }
    }
}