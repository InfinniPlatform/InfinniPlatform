using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitTestComplexFillDocumentAction
    {
        public void Action(IActionContext target)
        {
            target.Item.PrefiledField = "TestValue";
        }
    }
}