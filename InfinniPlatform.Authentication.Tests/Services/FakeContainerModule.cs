using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Authentication.Tests.Services
{
    internal sealed class FakeContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterHttpServices(GetType().Assembly);
        }
    }
}