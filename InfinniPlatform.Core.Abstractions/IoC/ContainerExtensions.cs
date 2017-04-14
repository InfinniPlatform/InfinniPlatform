using System;
using System.Reflection;

namespace InfinniPlatform.Sdk.IoC
{
    public static class ContainerExtensions
    {
        /// <summary>
        /// Регистрирует указанные типы текущей сборки.
        /// </summary>
        /// <example>
        /// <code>
        /// RegisterAssemblyTypes(assembly,
        ///   t => typeof(IHttpService).IsAssignableFrom(t),
        ///   r => r.As&lt;IHttpService&gt;().SingleInstance());
        /// </code>
        /// </example>
        public static void RegisterAssemblyTypes(this IContainerBuilder builder, Assembly assembly, Func<Type, bool> typeSelector, Action<IContainerRegistrationRule> registrationRule = null)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (typeSelector == null)
            {
                throw new ArgumentNullException(nameof(typeSelector));
            }

            if (registrationRule == null)
            {
                registrationRule = r => r.SingleInstance();
            }

            foreach (var type in assembly.GetTypes())
            {
                if (type.GetTypeInfo().IsClass && !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsGenericType && typeSelector(type))
                {
                    registrationRule(builder.RegisterType(type));
                }
            }
        }
    }
}