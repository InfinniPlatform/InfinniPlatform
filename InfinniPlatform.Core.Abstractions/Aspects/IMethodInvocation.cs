using System;
using System.Collections.Generic;
using System.Reflection;

namespace InfinniPlatform.Aspects
{
    /// <summary>
    /// Invocation information of proxied method.
    /// </summary>
    public interface IMethodInvocation
    {
        /// <summary>
        /// Invoked method properties.
        /// </summary>
        MethodInfo MethodInvocationTarget { get; }

        /// <summary>
        /// Invoked object.
        /// </summary>
        object InvocationTarget { get; }

        /// <summary>
        /// Arguments values of invoked method.
        /// </summary>
        IEnumerable<object> Arguments { get; }

        /// <summary>
        /// Return value of invoked method.
        /// </summary>
        object ReturnValue { get; }

        /// <summary>
        /// Proceeds the call to the next interceptor in line, and ultimately to the target method.
        /// </summary>
        void Proceed();

        /// <summary>
        /// Raised when invocation throws an exception.
        /// </summary>
        event Action<Exception> OnError;

        /// <summary>
        /// Raised after successful invocation.
        /// </summary>
        event Action OnSuccess;
    }
}