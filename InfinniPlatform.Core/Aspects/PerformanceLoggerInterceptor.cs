using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
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
            var isAssignableFromTask = typeof(Task).IsAssignableFrom(invocation.MethodInvocationTarget.ReturnType);

            var targetType = invocation.InvocationTarget.GetType();
            var methodName = invocation.MethodInvocationTarget.Name;

            if (isAssignableFromTask)
            {
                var startTime = DateTime.Now;

                invocation.Proceed();

                var returnValue = (Task) invocation.ReturnValue;
                returnValue.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        GetPerfLogger(targetType).Log(methodName, startTime, t.Exception);
                    }
                    else
                    {
                        GetPerfLogger(targetType).Log(methodName, startTime);
                    }
                });
            }
            else
            {
                var startTime = DateTime.Now;

                try
                {
                    invocation.Proceed();

                    GetPerfLogger(targetType).Log(methodName, startTime);
                }
                catch (Exception e)
                {
                    GetPerfLogger(targetType).Log(methodName, startTime, e);
                }
            }
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