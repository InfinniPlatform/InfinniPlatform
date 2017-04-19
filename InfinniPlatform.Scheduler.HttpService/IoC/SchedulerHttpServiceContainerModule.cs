using InfinniPlatform.Core.Http;
using InfinniPlatform.Core.IoC;

namespace InfinniPlatform.Scheduler.IoC
{
    public class SchedulerHttpServiceContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<SchedulerHttpService>().As<IHttpService>().SingleInstance();
        }
    }
}