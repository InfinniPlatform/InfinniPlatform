using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class YetAnotherTestComplexAction
    {
        public YetAnotherTestComplexAction(DocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly DocumentApi _documentApi;

        public void Action(IApplyContext target)
        {
            if (target.Item.TestValue != "AnotherTest" &&
                target.Item.RegisterMoveValue != "RegisterMove")
            {
                dynamic item = new DynamicWrapper();
                item.TestValue = "AnotherTest";
                _documentApi.SetDocument(target.Configuration, target.Metadata, item);
            }
        }
    }
}