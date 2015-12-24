using System;
using System.Reflection;

using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.Sdk.IoC
{
    public static class ContainerExtensions
    {
        private const string ActionUnitPrefix = "ActionUnit";
        private const string ValidationUnitPrefix = "ValidationUnit";

        /// <summary>
        /// Регистрирует  все прикладные скрипты текущей сборки.
        /// </summary>
        /// <remarks>
        /// Прикладные скрипты будут зарегистрированы с правилами .AsSelf().SingleInstance().
        /// </remarks>
        /// <example>
        /// RegisterActionUnits(GetType().Assembly)
        /// </example>
        public static void RegisterActionUnits(this IContainerBuilder builder, Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && !type.IsGenericType
                    && (type.Name.IndexOf(ActionUnitPrefix, StringComparison.OrdinalIgnoreCase) >= 0
                        || type.Name.IndexOf(ValidationUnitPrefix, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    builder.RegisterType(type).AsSelf().SingleInstance();
                }
            }
        }

        public static void RegisterActionUnits(this IContainerBuilder builder)
        {
            
        }
    }
}