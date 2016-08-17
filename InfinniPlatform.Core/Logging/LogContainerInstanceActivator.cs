using System;
using System.Linq;
using System.Reflection;

using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Core.Logging
{
    internal sealed class LogContainerInstanceActivator<T> : IContainerInstanceActivator
    {
        public LogContainerInstanceActivator(Func<Type, IJsonObjectSerializer, T> logFactory)
        {
            _logFactory = logFactory;
        }

        private readonly Func<Type, IJsonObjectSerializer, T> _logFactory;

        public void Activate(object instance, IContainerResolver resolver)
        {
            var instanceType = instance.GetType();

            var properties = instanceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                         .Where(p => p.PropertyType == typeof(T) && p.CanWrite
                                                     && p.GetIndexParameters().Length == 0);

            foreach (var propToSet in properties)
            {
                propToSet.SetValue(instance, _logFactory(instanceType, resolver.Resolve<IJsonObjectSerializer>()), null);
            }
        }
    }
}