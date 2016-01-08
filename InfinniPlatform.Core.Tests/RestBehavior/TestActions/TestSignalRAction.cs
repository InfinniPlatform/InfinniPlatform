using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class TestSignalRAction
    {
        public TestSignalRAction(IWebClientNotificationComponent webClientNotificationComponent)
        {
            _webClientNotificationComponent = webClientNotificationComponent;
        }

        private readonly IWebClientNotificationComponent _webClientNotificationComponent;

        public void Action(IApplyContext target)
        {
            dynamic testObject = new DynamicWrapper();
            testObject.TestProperty = "Hello world";

            _webClientNotificationComponent.Notify("routingKey", testObject.ToString());
        }
    }
}