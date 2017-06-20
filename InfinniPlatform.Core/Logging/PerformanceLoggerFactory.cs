using System;
using InfinniPlatform.IoC;

namespace InfinniPlatform.Logging
{
    public class PerformanceLoggerFactory : IPerformanceLoggerFactory
    {
        private readonly IContainerResolver _resolver;

        public PerformanceLoggerFactory(IContainerResolver resolver)
        {
            _resolver = resolver;
        }

        public IPerformanceLogger Create(Type targetType)
        {
            return (IPerformanceLogger) _resolver.Resolve(typeof(IPerformanceLogger<>).MakeGenericType(targetType));
        }

        public IPerformanceLogger<T> Create<T>() where T : class
        {
            return _resolver.Resolve<IPerformanceLogger<T>>();
        }
    }
}