using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using InfinniPlatform.Logging;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Aspects
{
    public class PerformanceLoggerInterceptor : IAspectInterceptor
    {
        private readonly ILogger<PerformanceLoggerInterceptor> _logger;
        private readonly ConcurrentDictionary<Type, IPerformanceLogger> _perfLoggerCache;
        private readonly IPerformanceLoggerFactory _perfLoggerFactory;

        public PerformanceLoggerInterceptor(IPerformanceLoggerFactory perfLoggerFactory, ILogger<PerformanceLoggerInterceptor> logger)
        {
            _perfLoggerFactory = perfLoggerFactory;
            _logger = logger;
            _perfLoggerCache = new ConcurrentDictionary<Type, IPerformanceLogger>();
        }

        public void Intercept(IMethodInvocation invocation)
        {
            var isAssignableFromTask = typeof(Task).IsAssignableFrom(invocation.MethodInvocationTarget.ReturnType);

            if (isAssignableFromTask)
            {
                var startTime = DateTime.Now;

                invocation.Proceed();

                var returnValue = (Task) invocation.ReturnValue;
                returnValue.ContinueWith(t =>
                {
                    var startNew = Stopwatch.StartNew();
                    var perfLogger = GetPerfLogger(invocation.InvocationTarget.GetType());
                    var startNewElapsedMilliseconds = startNew.Elapsed.TotalMilliseconds;

                    _logger.LogInformation($"{startNewElapsedMilliseconds} ms to invoke _perfLoggerFactory.Create().");
                    perfLogger.Log(invocation.MethodInvocationTarget.Name, startTime);
                });
            }
            else
            {
                var startTime = DateTime.Now;

                invocation.Proceed();

                var perfLogger = GetPerfLogger(invocation.InvocationTarget.GetType());
                perfLogger.Log(invocation.MethodInvocationTarget.Name, startTime);
            }
        }

        private IPerformanceLogger GetPerfLogger(Type targetType)
        {
            if (_perfLoggerCache.ContainsKey(targetType))
            {
                return _perfLoggerCache[targetType];
            }

            var perfLogger = _perfLoggerFactory.Create(targetType);
            var tryAdd = _perfLoggerCache.TryAdd(targetType, perfLogger);
            return perfLogger;
        }
    }
}