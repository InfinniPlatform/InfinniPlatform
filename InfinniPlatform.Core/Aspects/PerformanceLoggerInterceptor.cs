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
        private readonly ConcurrentDictionary<Type, IPerformanceLogger> _perfLoggerCache;
        private readonly IPerformanceLoggerFactory _perfLoggerFactory;

        public PerformanceLoggerInterceptor(IPerformanceLoggerFactory perfLoggerFactory)
        {
            _perfLoggerFactory = perfLoggerFactory;
            _perfLoggerCache = new ConcurrentDictionary<Type, IPerformanceLogger>();
        }

        public void Intercept(IMethodInvocation invocation)
        {
            var targetType = invocation.InvocationTarget.GetType();
            var methodName = invocation.MethodInvocationTarget.Name;

            var startTime = DateTime.Now;

            invocation.OnError += exception => GetPerfLogger(targetType).Log(methodName, startTime, exception);
            invocation.OnSuccess += () => GetPerfLogger(targetType).Log(methodName, startTime);

            invocation.Proceed();
        }

        private IPerformanceLogger GetPerfLogger(Type targetType)
        {
            if (_perfLoggerCache.ContainsKey(targetType))
            {
                return _perfLoggerCache[targetType];
            }

            var perfLogger = _perfLoggerFactory.Create(targetType);
            _perfLoggerCache.TryAdd(targetType, perfLogger);
            return perfLogger;
        }
    }
}