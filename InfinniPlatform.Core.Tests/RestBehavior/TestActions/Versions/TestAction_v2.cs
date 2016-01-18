using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions.Versions
{
    public sealed class TestAction_v2
    {
        public TestAction_v2(DocumentApi documentApi)
        {
            _documentApi = documentApi;
        }

        private readonly DocumentApi _documentApi;

        public void Action(IActionContext target)
        {
            if (target.Item.Name != "Name_TestAction_v2")
            {
                dynamic testDoc1 = new DynamicWrapper();
                testDoc1.Name = "Name_TestAction_v2";

                _documentApi.SetDocument(target.Configuration, target.Metadata, testDoc1);
            }
        }
    }
}