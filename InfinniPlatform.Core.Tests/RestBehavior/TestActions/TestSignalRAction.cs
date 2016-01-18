using InfinniPlatform.Core.ClientNotification;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class TestSignalRAction
    {
        public TestSignalRAction(IWebClientNotificationService webClientNotificationComponent)
        {
            _webClientNotificationComponent = webClientNotificationComponent;
        }

        private readonly IWebClientNotificationService _webClientNotificationComponent;

        public void Action(IActionContext target)
        {
            dynamic testObject = new DynamicWrapper();
            testObject.TestProperty = "Hello world";

            _webClientNotificationComponent.Notify("routingKey", testObject.ToString());
        }
    }
}