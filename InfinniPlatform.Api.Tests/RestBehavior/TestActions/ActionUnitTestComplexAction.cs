using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class ActionUnitTestComplexAction
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.TestValue != "Test" &&
                target.Item.RegisterMoveValue != "RegisterMove")
            {
                dynamic item = new DynamicWrapper();
                item.TestValue = "Test";
                new DocumentApi().SetDocument(target.Item.Configuration, target.Item.Metadata, item);
            }
        }
    }
}