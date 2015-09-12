using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions.Versions
{
    public sealed class TestAction
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.Name != "Name_TestAction")
            {
                dynamic testDoc1 = new DynamicWrapper();
                testDoc1.Name = "Name_TestAction";
                target.Context.GetComponent<IDocumentApi>()
                      .SetDocument(target.Item.Configuration, target.Item.Metadata, testDoc1);
            }
        }
    }
}