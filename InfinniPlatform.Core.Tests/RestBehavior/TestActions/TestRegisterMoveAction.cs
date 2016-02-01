using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class TestRegisterMoveAction
    {
        public TestRegisterMoveAction(IDocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly IDocumentApi _documentApi;

        public void Action(IActionContext target)
        {
            if (target.Item.RegisterMoveValue != "RegisterMove" &&
                target.Item.TestValue != "Test")
            {
                dynamic item = new DynamicWrapper();
                item.RegisterMoveValue = "RegisterMove";
                _documentApi.SetDocument(target.Configuration, target.DocumentType, item);
            }
        }
    }
}