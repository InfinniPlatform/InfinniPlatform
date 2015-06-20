using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class TestComplexAction
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.TestValue != "Test" &&
                target.Item.RegisterMoveValue != "RegisterMove")
            {
                dynamic item = new DynamicWrapper();
                item.TestValue = "Test";
                new DocumentApi(target.Version).SetDocument(target.Item.Configuration, target.Item.Metadata, item);
            }
        }
    }
}