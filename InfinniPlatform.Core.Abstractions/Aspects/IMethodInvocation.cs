using System.Collections.Generic;
using System.Reflection;

namespace InfinniPlatform.Aspects
{
    public interface IMethodInvocation
    {
        MethodInfo MethodInvocationTarget { get; }

        object InvocationTarget { get; }

        IEnumerable<object> Arguments { get; }

        object ReturnValue { get; }

        //Exception Exception { get; }

        void Proceed();
    }
}