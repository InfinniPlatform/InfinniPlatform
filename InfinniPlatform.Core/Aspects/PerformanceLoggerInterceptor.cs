using System;
using System.Collections.Concurrent;
using InfinniPlatform.Logging;

namespace InfinniPlatform.Aspects
{
    /// <summary>
    /// Interceptor for logging execution time of invoked method.
    /// </summary>
    public class PerformanceLoggerInterceptor : IMethodInterceptor
    {
        private readonly IPerformanceLoggerFactory _perfLoggerFactory;
        private readonly ConcurrentDictionary<Type, IPerformanceLogger> _perfLoggerCache;

        /// <summary>
        /// Initializes a new instance of <see cref="PerformanceLoggerInterceptor" />. 
        /// </summary>
        /// <param name="perfLoggerFactory">Factory for creating <see cref="IPerformanceLoggerFactory"/>.</param>
        public PerformanceLoggerInterceptor(IPerformanceLoggerFactory perfLoggerFactory)
        {
            _perfLoggerFactory = perfLoggerFactory;
            _perfLoggerCache = new ConcurrentDictionary<Type, IPerformanceLogger>();
        }

        /// <inheritdoc />
        public void Intercept(IMethodInvocation invocation)
        {
            var targetType = invocation.InvocationTarget.GetType();
            var methodName = invocation.MethodInvocationTarget.Name;

            var startTime = DateTime.Now;

            invocation.OnError += exception => GetPerfLogger(targetType).Log(methodName, startTime, exception);
            invocation.OnSuccess += returnValue => GetPerfLogger(targetType).Log(methodName, startTime);

            invocation.Proceed();
        }

        private IPerformanceLogger GetPerfLogger(Type targetType)
        {
            if (!_perfLoggerCache.TryGetValue(targetType, out var perfLogger))
            {
                perfLogger = _perfLoggerFactory.Create(targetType);

                perfLogger = _perfLoggerCache.GetOrAdd(targetType, perfLogger);
            }

            return perfLogger;
        }
    }
}