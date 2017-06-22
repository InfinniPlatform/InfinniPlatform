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
    public class MethodInvocation : IMethodInvocation
    {
        private readonly IInvocation _invocation;

        public MethodInvocation(IInvocation invocation)
        {
            _invocation = invocation;
        }

        public object InvocationTarget => _invocation.InvocationTarget;
        public MethodInfo MethodInvocationTarget => _invocation.MethodInvocationTarget;
        public IEnumerable<object> Arguments => _invocation.Arguments;
        public object ReturnValue => _invocation.ReturnValue;

        public void Proceed()
        {
            var isAssignableFromTask = typeof(Task).IsAssignableFrom(_invocation.MethodInvocationTarget.ReturnType);

            if (isAssignableFromTask)
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

                var returnValue = (Task) _invocation.ReturnValue;
                returnValue.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        OnError?.Invoke(t.Exception);
                    }
                    else
                    {
                        OnSuccess?.Invoke();
                    }
                });
            }
            else
            {
                try
                {
                    _invocation.Proceed();

                    OnSuccess?.Invoke();
                }
                catch (Exception e)
                {
                    OnError?.Invoke(e);
                }
            }
        }

        public event Action<Exception> OnError;
        public event Action OnSuccess;
    }
}