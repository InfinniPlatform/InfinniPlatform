using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;

namespace InfinniPlatform.Aspects
{
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
            _invocation.Proceed();
        }
    }
}