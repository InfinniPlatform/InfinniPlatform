using System;
using System.Reflection;

using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Core.Logging
{
    internal sealed class LogContainerParameterResolver<T> : IContainerParameterResolver
    {
        public LogContainerParameterResolver(Func<Type, T> logFactory)
        {
            _logFactory = logFactory;
        }

        private readonly Func<Type, T> _logFactory;

        public bool CanResolve(ParameterInfo parameterInfo, IContainerResolver resolver)
        {
            return parameterInfo.ParameterType == typeof(T);
        }

        public object Resolve(ParameterInfo parameterInfo, IContainerResolver resolver)
        {
            return _logFactory(parameterInfo.Member.DeclaringType);
        }
    }
}