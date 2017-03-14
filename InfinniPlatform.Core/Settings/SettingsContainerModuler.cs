using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Core.Settings
{
    internal class SettingsContainerModuler : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<AppConfiguration>()
                   .As<IAppConfiguration>()
                   .SingleInstance();

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<AppEnvironment>(AppEnvironment.SectionName))
                   .As<IAppEnvironment>()
                   .SingleInstance();
        }
    }
}