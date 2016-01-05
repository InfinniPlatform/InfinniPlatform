using System.Linq;
using System.Reflection;

using InfinniPlatform.Sdk.IoC;

using log4net;

namespace InfinniPlatform.Core.Logging
{
    internal sealed class Log4NetContainerInstanceActivator : IContainerInstanceActivator
    {
        public void Activate(object instance, IContainerResolver resolver)
        {
            var instanceType = instance.GetType();

            var properties = instanceType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                         .Where(p => p.PropertyType == typeof(ILog) && p.CanWrite
                                                     && p.GetIndexParameters().Length == 0);

            foreach (var propToSet in properties)
            {
                propToSet.SetValue(instance, Log4NetLogManagerCache.GetLog(instanceType), null);
            }
        }
    }
}