using System;

namespace InfinniPlatform.Logging
{
    public interface IPerformanceLoggerFactory
    {
        IPerformanceLogger Create(Type targetType);
        IPerformanceLogger<T> Create<T>() where T : class;
    }
}