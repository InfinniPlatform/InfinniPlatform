using InfinniPlatform.Http;
using InfinniPlatform.IoC;

namespace InfinniPlatform.Heartbeat.IoC
{
    public class HeartbeatHttpServiceContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<HeartbeatHttpService>().As<IHttpService>().SingleInstance();
        }
    }
}