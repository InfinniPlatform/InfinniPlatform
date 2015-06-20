using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions.Versions
{
    public sealed class TestAction_v2
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.Name != "Name_TestAction_v2")
            {
                dynamic testDoc1 = new DynamicWrapper();
                testDoc1.Name = "Name_TestAction_v2";
                target.Context.GetComponent<DocumentApi>(target.Version)
                      .SetDocument(target.Item.Configuration, target.Item.Metadata, testDoc1);
            }
        }
    }
}