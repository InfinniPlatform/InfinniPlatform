using InfinniPlatform.Heartbeat.Settings;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Heartbeat.IoC
{
    public class HeartbeatContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<HeartbeatAppEventHandler>()
                   .As<IAppEventHandler>()
                   .SingleInstance();

            builder.RegisterFactory(GetAgentSettings)
                   .As<HeartbeatSettings>()
                   .SingleInstance();
        }

        private static HeartbeatSettings GetAgentSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<HeartbeatSettings>(HeartbeatSettings.SectionName);
        }
    }
}