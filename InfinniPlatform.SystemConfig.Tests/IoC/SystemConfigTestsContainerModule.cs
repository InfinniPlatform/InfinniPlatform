using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.SystemConfig.Tests.IoC
{
    internal sealed class SystemConfigTestsContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Прикладные скрипты
            builder.RegisterActionUnits();
        }
    }
}