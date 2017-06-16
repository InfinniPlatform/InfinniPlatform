using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace InfinniPlatform.ServiceHost.Interception
{
    public class CallLogger : IInterceptor
    {
        private readonly ILogger<CallLogger> _logger;

        public CallLogger(ILogger<CallLogger> logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            LoggerExtensions.LogInformation(_logger, $"Calling method {invocation.Method.Name}.");
            var returnType = invocation.MethodInvocationTarget.ReturnType;

            var isAssignableFromTask = typeof(Task).IsAssignableFrom(returnType);

            if (isAssignableFromTask)
            {
                //var task = invocation.ReturnValue as Task;
                var stopwatch = Stopwatch.StartNew();
                invocation.Proceed();
                var returnValue = (Task)invocation.ReturnValue;
                returnValue.ContinueWith(t =>
                {
                    var elapsed = stopwatch.ElapsedMilliseconds;
                    LoggerExtensions.LogInformation(_logger, $"Done in {elapsed} ms.");
                });
            }
            else
            {
                var stopwatch = Stopwatch.StartNew();
                invocation.Proceed();
                var elapsed = stopwatch.ElapsedMilliseconds;
                LoggerExtensions.LogInformation(_logger, $"Done in {elapsed} ms.");
            }
        }
    }
}