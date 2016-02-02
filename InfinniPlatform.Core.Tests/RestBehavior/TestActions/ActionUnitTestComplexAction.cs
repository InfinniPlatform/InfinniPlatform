using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitTestComplexAction
    {
        public ActionUnitTestComplexAction(IDocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly IDocumentApi _documentApi;

        public void Action(IActionContext target)
        {
            if (target.Item.TestValue != "Test" && target.Item.RegisterMoveValue != "RegisterMove")
            {
                string documentType = target.DocumentType;

                dynamic documentInstance = new DynamicWrapper();
                documentInstance.TestValue = "Test";

                _documentApi.SetDocument(documentType, documentInstance);
            }
        }
    }
}