using System;

namespace InfinniPlatform.Logging
{
    /// <summary>
    /// Factory for creating <see cref="IPerformanceLogger"/> instances.
    /// </summary>
    public interface IPerformanceLoggerFactory
    {
        /// <summary>
        /// Creates <see cref="IPerformanceLogger"/> instance for specified event source.
        /// </summary>
        /// <param name="targetType">The event source.</param>
        IPerformanceLogger Create(Type targetType);

        /// <summary>
        /// Creates <see cref="IPerformanceLogger"/> instance for specified event source.
        /// </summary>
        /// <typeparam name="TComponent">The event source.</typeparam>
        IPerformanceLogger<TComponent> Create<TComponent>() where TComponent : class;
    }
}