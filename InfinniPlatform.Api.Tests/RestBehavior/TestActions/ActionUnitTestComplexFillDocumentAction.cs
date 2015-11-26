using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitTestComplexFillDocumentAction
    {
        public void Action(IApplyContext target)
        {
            target.Item.PrefiledField = "TestValue";
        }
    }
}