using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions.Versions
{
    public sealed class TestAction_v1
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.Name != "Name_TestAction_v1")
            {
                dynamic testDoc1 = new DynamicWrapper();
                testDoc1.Name = "Name_TestAction_v1";

                target.Context.GetComponent<DocumentApi>()
                      .SetDocument(target.Configuration, target.Metadata, testDoc1);
            }
        }
    }
}