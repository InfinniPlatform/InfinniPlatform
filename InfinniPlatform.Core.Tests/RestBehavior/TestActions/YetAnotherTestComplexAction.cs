using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class YetAnotherTestComplexAction
    {
        public YetAnotherTestComplexAction(IDocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly IDocumentApi _documentApi;

        public void Action(IActionContext target)
        {
            if (target.Item.TestValue != "AnotherTest" &&
                target.Item.RegisterMoveValue != "RegisterMove")
            {
                dynamic item = new DynamicWrapper();
                item.TestValue = "AnotherTest";
                _documentApi.SetDocument(target.Configuration, target.DocumentType, item);
            }
        }
    }
}