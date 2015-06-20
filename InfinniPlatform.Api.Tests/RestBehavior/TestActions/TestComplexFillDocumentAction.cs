using InfinniPlatform.Sdk.Application.Contracts;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class TestComplexFillDocumentAction
    {
        public void Action(IApplyContext target)
        {
            target.Item.PrefiledField = "TestValue";
        }
    }
}