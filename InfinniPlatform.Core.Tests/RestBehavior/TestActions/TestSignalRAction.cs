using InfinniPlatform.Sdk.ClientNotification;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Core.Tests.RestBehavior.TestActions
{
    public sealed class TestSignalRAction
    {
        public TestSignalRAction(IClientNotificationService clientNotificationService)
        {
            _clientNotificationService = clientNotificationService;
        }

        private readonly IClientNotificationService _clientNotificationService;

        public void Action(IActionContext target)
        {
            dynamic testObject = new DynamicWrapper();
            testObject.TestProperty = "Hello world";

            _clientNotificationService.Notify("routingKey", testObject.ToString());
        }
    }
}