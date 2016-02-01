using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ValidationUnitTestComplexValidator
    {
        public void Action(IActionContext target)
        {
            target.IsValid = false;
            target.ValidationMessage = "TestComplexValidatorMessage";
        }
    }
}