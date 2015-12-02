using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.SystemConfig.Administration.IoC
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