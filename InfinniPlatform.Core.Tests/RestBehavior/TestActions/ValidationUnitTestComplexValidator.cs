using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ValidationUnitTestComplexValidator
    {
        public void Action(IApplyContext target)
        {
            target.IsValid = false;
            target.ValidationMessage = "TestComplexValidatorMessage";
        }
    }
}