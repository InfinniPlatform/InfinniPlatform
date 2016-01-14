using System;
using System.Reflection;

using InfinniPlatform.Sdk.Services;

namespace InfinniPlatform.Sdk.IoC
{
    public static class ContainerExtensions
    {
        private const string ActionUnitPrefix = "ActionUnit";
        private const string ValidationUnitPrefix = "ValidationUnit";

        /// <summary>
        /// Регистрирует все прикладные скрипты текущей сборки.
        /// </summary>
        /// <remarks>
        /// Прикладные скрипты будут зарегистрированы со стратегией SingleInstance().
        /// </remarks>
        /// <example>
        /// RegisterActionUnits(GetType().Assembly)
        /// </example>
        public static void RegisterActionUnits(this IContainerBuilder builder, Assembly assembly)
        {
            RegisterAssemblyTypes(builder, assembly,
                t => (t.Name.IndexOf(ActionUnitPrefix, StringComparison.OrdinalIgnoreCase) >= 0
                      || t.Name.IndexOf(ValidationUnitPrefix, StringComparison.OrdinalIgnoreCase) >= 0),
                r => r.AsSelf().SingleInstance());
        }

        /// <summary>
        /// Регистрирует все прикладные сервисы текущей сборки.
        /// </summary>
        /// <remarks>
        /// Прикладные скрипты будут зарегистрированы со стратегией SingleInstance().
        /// </remarks>
        /// <example>
        /// RegisterHttpServices(GetType().Assembly)
        /// </example>
        public static void RegisterHttpServices(this IContainerBuilder builder, Assembly assembly)
        {
            RegisterAssemblyTypes(builder, assembly, 
                t => typeof(IHttpService).IsAssignableFrom(t),
                r => r.As<IHttpService>().SingleInstance());
        }

        /// <summary>
        /// Регистрирует указанные типы текущей сборки.
        /// </summary>
        /// <example>
        /// RegisterAssemblyTypes(builder, assembly,
        ///    t => typeof(IHttpService).IsAssignableFrom(t),
        ///    r => r.As&lt;IHttpService&gt;().SingleInstance());
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
                if (type.IsClass && !type.IsAbstract && !type.IsGenericType && typeSelector(type))
                {
                    registrationRule(builder.RegisterType(type));
                }
            }
        }
    }
}