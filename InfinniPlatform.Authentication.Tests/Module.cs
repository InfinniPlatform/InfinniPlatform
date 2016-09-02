using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.Authentication.Tests
{
    internal sealed class Module : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterConsumers(typeof(Module).Assembly);
        }
    }
}