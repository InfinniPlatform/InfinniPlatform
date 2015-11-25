using System.Reflection;

using InfinniPlatform.Sdk.IoC;

using log4net;

namespace InfinniPlatform.Logging
{
    internal sealed class Log4NetContainerParameterResolver : IContainerParameterResolver
    {
        public bool CanResolve(ParameterInfo parameterInfo, IContainerResolver resolver)
        {
            return parameterInfo.ParameterType == typeof(ILog);
        }

        public object Resolve(ParameterInfo parameterInfo, IContainerResolver resolver)
        {
            return Log4NetLogManagerCache.GetLog(parameterInfo.Member.DeclaringType);
        }
    }
}