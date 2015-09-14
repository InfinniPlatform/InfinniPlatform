using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class TestRegisterMoveAction
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.RegisterMoveValue != "RegisterMove" &&
                target.Item.TestValue != "Test")
            {
                dynamic item = new DynamicWrapper();
                item.RegisterMoveValue = "RegisterMove";
                new DocumentApi().SetDocument(target.Item.Configuration, target.Item.Metadata, item);
            }
        }
    }
}