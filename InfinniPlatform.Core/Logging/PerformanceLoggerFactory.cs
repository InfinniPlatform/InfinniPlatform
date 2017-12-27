using System;
using InfinniPlatform.IoC;

namespace InfinniPlatform.Logging
{
    /// <inheritdoc />
    public class PerformanceLoggerFactory : IPerformanceLoggerFactory
    {
        private readonly IContainerResolver _resolver;

        /// <summary>
        /// Initializes a new instance of <see cref="PerformanceLoggerFactory" />.
        /// </summary>
        /// <param name="resolver">Application container resolver.</param>
        public PerformanceLoggerFactory(IContainerResolver resolver)
        {
            _resolver = resolver;
        }

        /// <inheritdoc />
        public IPerformanceLogger Create(Type targetType)
        {
            return (IPerformanceLogger) _resolver.Resolve(typeof(IPerformanceLogger<>).MakeGenericType(targetType));
        }

        /// <inheritdoc />
        public IPerformanceLogger<T> Create<T>() where T : class
        {
            return _resolver.Resolve<IPerformanceLogger<T>>();
        }
    }
}