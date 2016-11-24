using InfinniPlatform.Sdk.Http.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;
using InfinniPlatform.ViewEngine.Settings;

using Nancy.Bootstrapper;

namespace InfinniPlatform.ViewEngine.IoC
{
    /// <summary>
    /// Регистрация компонентов в IoC-контейнере.
    /// </summary>
    public sealed class WatcherContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            //Hosting 

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>()
                                          .GetSection<ViewEngineSettings>(ViewEngineSettings.SectionName))
                   .As<ViewEngineSettings>()
                   .SingleInstance();

            builder.RegisterType<NancyBootstrapper>()
                   .As<INancyBootstrapper>()
                   .SingleInstance();

            builder.RegisterHttpServices(typeof(WatcherContainerModule).Assembly);
        }
    }
}