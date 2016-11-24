using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Watcher.IoC
{
    /// <summary>
    /// Регистрация компонентов в IoC-контейнере.
    /// </summary>
    public sealed class WatcherContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            //Hosting 

            builder.RegisterType<Watcher>()
                   .As<IAppEventHandler>()
                   .SingleInstance();

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>()
                                          .GetSection<WatcherSettings>(WatcherSettings.SectionName))
                   .As<WatcherSettings>()
                   .SingleInstance();
        }
    }
}