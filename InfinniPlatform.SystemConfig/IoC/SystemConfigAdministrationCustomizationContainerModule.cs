using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.SystemConfig.IoC
{
    internal sealed class SystemConfigAdministrationCustomizationContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Прикладные скрипты
            builder.RegisterActionUnits(GetType().Assembly);
        }
    }
}