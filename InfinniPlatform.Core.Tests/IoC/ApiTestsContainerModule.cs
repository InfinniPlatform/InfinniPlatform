using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Core.Tests.IoC
{
    internal sealed class ApiTestsContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterActionUnits(GetType().Assembly);
        }
    }
}