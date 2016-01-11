using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Core.Hosting
{
    public class MethodInvokationInfo
    {
        private readonly MethodInfo _methodInfo;
        private readonly List<ReflectedParameterInfo> _parameters = new List<ReflectedParameterInfo>();
        private readonly IEnumerable<string> _serviceNames;
        private readonly IQueryHandler _target;
        private readonly VerbType _verbType;

        /// <summary>
        ///     Конструктор вызова метода обработчика запроса
        /// </summary>
        /// <param name="methodInfo">Метод обработчика</param>
        /// <param name="target">Экземпляр обработчика</param>
        /// <param name="verbType">Тип http-верба</param>
        /// <param name="serviceNames">Список зарегистрированных имен вызова обработчика </param>
        public MethodInvokationInfo(MethodInfo methodInfo, IQueryHandler target, VerbType verbType,
            IEnumerable<string> serviceNames)
        {
            _methodInfo = methodInfo;
            _target = target;
            _verbType = verbType;
            _serviceNames = serviceNames.Select(s => s.ToLowerInvariant()).ToList();
            _parameters = methodInfo.GetParameters().Select(p => new ReflectedParameterInfo(p)).ToList();
        }

        public IQueryHandler TargetType
        {
            get { return _target; }
        }

        internal VerbType VerbType
        {
            get { return _verbType; }
        }

        public bool CanVerb(string serviceName, VerbType verbType)
        {
            if (VerbType != verbType)
            {
                return false;
            }

            if (!_serviceNames.Contains(serviceName.ToLowerInvariant()) && !string.IsNullOrEmpty(serviceName))
            {
                return false;
            }

            return true;
        }

        public TargetDelegate ConstructDelegate(IDictionary<string, object> verbArguments, object instance,
            HttpResultHandlerType httpResultHandler)
        {
            var arguments = new List<object>();


            foreach (var parameterInfo in _parameters)
            {
                var param = verbArguments.FirstOrDefault(v => parameterInfo.Named(v.Key));
                if (param.Equals(default(KeyValuePair<string, object>)))
                {
                    //если параметр имеет значения по умолчанию, то устанавливаем
                    if (parameterInfo.HasDefaultValue)
                    {
                        arguments.Add(parameterInfo.GetArgumentValue(parameterInfo.DefaultValue));
                        continue;
                    }

                    throw new ArgumentException(string.Format("parameter \"{0}\" not found!", parameterInfo.Name));
                }

                arguments.Add(parameterInfo.GetArgumentValue(param.Value));
            }

            return new TargetDelegate(_methodInfo, instance, arguments.ToArray(), httpResultHandler);
        }
    }
}