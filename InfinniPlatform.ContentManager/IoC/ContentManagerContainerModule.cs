using InfinniPlatform.ContentManager.Bundling;
using InfinniPlatform.ContentManager.Settings;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.ContentManager.IoC
{
    internal class ContentManagerContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Settings

            builder.RegisterFactory(GetContentSettings)
                   .As<ContentSettings>()
                   .SingleInstance();

            // Hosting

            builder.RegisterType<AssetsInitializer>()
                   .As<IAppEventHandler>()
                   .SingleInstance();
        }


        private static ContentSettings GetContentSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<ContentSettings>(ContentSettings.SectionName);
        }
    }
}