using System;
using System.Reflection;

using InfinniPlatform.IoC;
using InfinniPlatform.Serialization;

namespace InfinniPlatform.Logging.IoC
{
    public class LogContainerParameterResolver<T> : IContainerParameterResolver
    {
        public LogContainerParameterResolver(Func<Type, IJsonObjectSerializer, T> logFactory)
        {
            _logFactory = logFactory;
        }

        private readonly Func<Type, IJsonObjectSerializer, T> _logFactory;

        public bool CanResolve(ParameterInfo parameterInfo, Func<IContainerResolver> resolver)
        {
            return parameterInfo.ParameterType == typeof(T);
        }

        public object Resolve(ParameterInfo parameterInfo, Func<IContainerResolver> resolver)
        {
            return _logFactory(parameterInfo.Member.DeclaringType, resolver().Resolve<IJsonObjectSerializer>());
        }
    }
}