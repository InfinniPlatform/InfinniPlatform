using InfinniPlatform.Core.IoC;
using InfinniPlatform.Core.Logging;

namespace InfinniPlatform.Logging.IoC
{
    public class Log4NetContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.OnCreateInstance(new LogContainerParameterResolver<ILog>(LogManagerCache.GetLog));
            builder.OnActivateInstance(new LogContainerInstanceActivator<ILog>(LogManagerCache.GetLog));

            builder.OnCreateInstance(new LogContainerParameterResolver<IPerformanceLog>(LogManagerCache.GetPerformanceLog));
            builder.OnActivateInstance(new LogContainerInstanceActivator<IPerformanceLog>(LogManagerCache.GetPerformanceLog));
        }
    }
}