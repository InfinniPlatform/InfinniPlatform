using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Logging;

namespace InfinniPlatform.Core.Logging
{
    internal class LoggingContainerModule : IContainerModule
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