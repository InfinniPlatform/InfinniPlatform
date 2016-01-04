using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.SystemConfig.IoC
{
    internal sealed class SystemConfigAdministrationContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Прикладные скрипты
            builder.RegisterActionUnits(GetType().Assembly);
        }
    }
}