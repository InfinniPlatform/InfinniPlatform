using System;
using System.Reflection;
using System.Threading.Tasks;
using InfinniPlatform.Logging;

namespace InfinniPlatform.Aspects
{
    public class PerformanceLoggerInterceptor : IAspectInterceptor
    {
        private readonly IPerformanceLoggerFactory _perfLoggerFactory;

        public PerformanceLoggerInterceptor(IPerformanceLoggerFactory perfLoggerFactory)
        {
            _perfLoggerFactory = perfLoggerFactory;
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
                    var perfLogger = _perfLoggerFactory.Create(invocation.InvocationTarget.GetType());
                    perfLogger.Log(invocation.MethodInvocationTarget.Name, startTime);
                });
            }
            else
            {
                var startTime = DateTime.Now;

                invocation.Proceed();

                var perfLogger = _perfLoggerFactory.Create(invocation.InvocationTarget.GetType());
                perfLogger.Log(invocation.MethodInvocationTarget.Name, startTime);
            }
        }
    }
}