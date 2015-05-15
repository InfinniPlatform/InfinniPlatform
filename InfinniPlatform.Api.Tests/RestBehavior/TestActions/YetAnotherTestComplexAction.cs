using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class YetAnotherTestComplexAction
    {
        public void Action(IApplyContext target)
        {
            if (target.Item.TestValue != "AnotherTest" &&
                target.Item.RegisterMoveValue != "RegisterMove")
            {
                dynamic item = new DynamicWrapper();
                item.TestValue = "AnotherTest";
                new DocumentApi().SetDocument(target.Item.Configuration, target.Item.Metadata, item);
            }
        }
    }
}