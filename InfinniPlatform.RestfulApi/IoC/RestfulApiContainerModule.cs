using InfinniPlatform.Modules;
using InfinniPlatform.RestfulApi.Installers;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.RestfulApi.IoC
{
    internal sealed class RestfulApiContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<RestfulApiInstaller>()
                   .As<IModuleInstaller>()
                   .SingleInstance();

            // Прикладные скрипты
            builder.RegisterActionUnits(GetType().Assembly);
        }
    }
}