using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Api.Tests.IoC
{
    internal sealed class ApiTestsContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterActionUnits();
        }
    }
}