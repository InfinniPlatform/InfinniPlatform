using InfinniPlatform.Scheduler.Contract;
using InfinniPlatform.Scheduler.Implementation;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Scheduler.IoC
{
    /// <summary>
    /// Модуль регистрации зависимостей <see cref="InfinniPlatform.Scheduler"/>.
    /// </summary>
    public class SchedulerContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<JobHandlerTypeSerializer>()
                   .As<IJobHandlerTypeSerializer>()
                   .SingleInstance();

            builder.RegisterType<JobInfoFactory>()
                   .As<IJobInfoFactory>()
                   .SingleInstance();
        }
    }
}