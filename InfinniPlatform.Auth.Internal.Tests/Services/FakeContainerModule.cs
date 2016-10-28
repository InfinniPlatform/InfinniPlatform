using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Auth.Internal.Tests.Services
{
    internal class FakeContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterHttpServices(GetType().Assembly);
        }
    }
}