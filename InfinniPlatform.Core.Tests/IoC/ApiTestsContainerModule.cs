using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Core.Tests.IoC
{
    internal sealed class ApiTestsContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterHttpServices(GetType().Assembly);
        }
    }
}