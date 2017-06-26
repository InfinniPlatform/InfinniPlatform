using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace InfinniPlatform.Aspects
{
    /// <summary>
    /// Invoked method properties.
    /// </summary>
    internal class MethodInvocation : IMethodInvocation
    {
        private readonly IInvocation _invocation;

        public MethodInvocation(IInvocation invocation)
        {
            _invocation = invocation;
        }

        public object InvocationTarget => _invocation.InvocationTarget;
        public MethodInfo MethodInvocationTarget => _invocation.MethodInvocationTarget;
        public IEnumerable<object> Arguments => _invocation.Arguments;

        public void Proceed()
        {
            try
            {
                _invocation.Proceed();
            }
            catch (Exception e)
            {
                OnError?.Invoke(e);
                return;
            }

            var returnValue = _invocation.ReturnValue;
            var returnValueAsTask = _invocation.ReturnValue as Task;

            if (returnValueAsTask == null)
            {
                OnSuccess?.Invoke(returnValue);
            }
            else
            {
                returnValueAsTask.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        OnError?.Invoke(t.Exception);
                    }
                    else
                    {
                        OnSuccess?.Invoke(returnValue);
                    }
                });
            }
        }

        public event Action<Exception> OnError;
        public event Action<object> OnSuccess;
    }
}