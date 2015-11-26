using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Sdk.Tests.IoC
{
    internal sealed class SdkTestsContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterActionUnits();
        }
    }
}