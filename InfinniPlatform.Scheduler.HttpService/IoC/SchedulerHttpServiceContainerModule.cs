using InfinniPlatform.Core.Abstractions.Http;
using InfinniPlatform.Core.Abstractions.IoC;

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