using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitGeneratorTest
    {
        public void Action(IApplyContext target)
        {
            target.Result = new DynamicWrapper();
            target.Result.TestValue = "Test";
        }
    }
}