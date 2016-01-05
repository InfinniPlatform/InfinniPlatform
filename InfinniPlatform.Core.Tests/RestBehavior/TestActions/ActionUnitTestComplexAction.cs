using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitTestComplexAction
    {
        public ActionUnitTestComplexAction(DocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly DocumentApi _documentApi;

        public void Action(IApplyContext target)
        {
            if (target.Item.TestValue != "Test" && target.Item.RegisterMoveValue != "RegisterMove")
            {
                string configuration = target.Configuration;
                string documentType = target.Metadata;

                dynamic documentInstance = new DynamicWrapper();
                documentInstance.TestValue = "Test";

                _documentApi.SetDocument(configuration, documentType, documentInstance);
            }
        }
    }
}