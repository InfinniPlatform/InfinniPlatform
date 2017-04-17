using System;
using System.Reflection;

using InfinniPlatform.Core.Abstractions.IoC;
using InfinniPlatform.Core.Abstractions.Serialization;

namespace InfinniPlatform.Log4NetAdapter
{
    internal sealed class LogContainerParameterResolver<T> : IContainerParameterResolver
    {
        public LogContainerParameterResolver(Func<Type, IJsonObjectSerializer, T> logFactory)
        {
            _logFactory = logFactory;
        }

        private readonly Func<Type, IJsonObjectSerializer, T> _logFactory;

        public bool CanResolve(ParameterInfo parameterInfo, IContainerResolver resolver)
        {
            return parameterInfo.ParameterType == typeof(T);
        }

        public object Resolve(ParameterInfo parameterInfo, IContainerResolver resolver)
        {
            return _logFactory(parameterInfo.Member.DeclaringType, resolver.Resolve<IJsonObjectSerializer>());
        }
    }
}