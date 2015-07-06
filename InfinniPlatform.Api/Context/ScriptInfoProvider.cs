using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Api.Context
{
    public sealed class ScriptInfoProvider
    {
        private readonly Assembly _assembly;

        public ScriptInfoProvider(Assembly assembly)
        {
            _assembly = assembly;
        }

        public IEnumerable<dynamic> GetScriptMethodsInfo()
        {
            return _assembly
                .GetProcessedMethods(new List<string> {"action", "validate"})
                .GetArgumentCountAccordingMethods()
                .GetArgumentAccordingTypeMethods()
                .GetMethodDynamicInfo();
        }
    }


    public static class ScriptInfoProviderExtensions
    {
        public static IEnumerable<MethodInfo> GetProcessedMethods(this Assembly assembly,
            IEnumerable<string> processedMethods)
        {
            var result = new List<MethodInfo>();
            foreach (var type in assembly.GetTypes())
            {
                foreach (var methodInfo in type.GetMethods())
                {
                    if (processedMethods.Contains(methodInfo.Name.ToLowerInvariant()))
                    {
                        result.Add(methodInfo);
                    }
                }
            }
            return result;
        }

        public static IEnumerable<MethodInfo> GetArgumentCountAccordingMethods(this IEnumerable<MethodInfo> methodInfos)
        {
            var result = new List<MethodInfo>();
            foreach (var methodInfo in methodInfos)
            {
                if (methodInfo.GetParameters().Count() == 1)
                {
                    result.Add(methodInfo);
                }
            }
            return result;
        }

        public static IEnumerable<MethodInfo> GetArgumentAccordingTypeMethods(this IEnumerable<MethodInfo> methodInfos)
        {
            var result = new List<MethodInfo>();
            foreach (var methodInfo in methodInfos)
            {
                var parameterType = methodInfo.GetParameters().First().ParameterType;
                var contextTypeKind = parameterType.GetContextTypeKind();
                if (contextTypeKind != ContextTypeKind.None)
                {
                    result.Add(methodInfo);
                }
            }
            return result;
        }

        public static IEnumerable<dynamic> GetMethodDynamicInfo(this IEnumerable<MethodInfo> methodInfoList)
        {
            var result = new List<dynamic>();
            foreach (var methodInfo in methodInfoList)
            {
                var argument = methodInfo.GetParameters().First().ParameterType;
                dynamic info = new DynamicWrapper();
                info.TypeName = methodInfo.ReflectedType.Name;
                info.MethodName = methodInfo.Name;
                info.ContextTypeDisplayName = argument.GetContextTypeVisual();
                info.ContextTypeCode = (int) argument.GetContextTypeKind();
                result.Add(info);
            }
            return result;
        }
    }
}