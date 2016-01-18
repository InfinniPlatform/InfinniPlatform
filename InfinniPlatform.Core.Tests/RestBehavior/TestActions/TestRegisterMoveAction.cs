using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class TestRegisterMoveAction
    {
        public TestRegisterMoveAction(DocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly DocumentApi _documentApi;

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