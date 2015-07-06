using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Tests.RestBehavior.TestActions
{
    public sealed class TestSignalRAction
    {
        public void Action(IApplyContext target)
        {
            dynamic testObject = new DynamicWrapper();
            testObject.TestProperty = "Hello world";

            target.Context.GetComponent<IWebClientNotificationComponent>()
                  .Notify("routingKey", testObject.ToString());
        }
    }
}