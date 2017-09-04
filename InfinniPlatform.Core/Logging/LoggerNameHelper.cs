using System;
using System.Reflection;
using System.Text;

namespace InfinniPlatform.Logging
{
    public static class LoggerNameHelper
    {
        /// <summary>
        /// Gets the category name for messages produced by the logger.
        /// </summary>
        /// <param name="componentType">The type of the logger event source.</param>
        /// <remarks>
        /// The method checks whether the given type is marked using the <see cref="LoggerNameAttribute" /> attribute.
        /// If it is marked returns <see cref="LoggerNameAttribute.Name" /> otherwise language representation of the type.
        /// If <paramref name="componentType"/> is generic this logic repeats recursively for each generic parameter.
        /// </remarks>
        public static string GetCategoryName(Type componentType)
        {
            return GetLoggerName(componentType);
        }

        /// <summary>
        /// Gets the category name for messages produced by the logger.
        /// </summary>
        /// <param name="componentType">The type of the logger event source.</param>
        /// <remarks>
        /// The method checks whether the given type is marked using the <see cref="LoggerNameAttribute" /> attribute.
        /// If it is marked returns <see cref="LoggerNameAttribute.Name" /> otherwise language representation of the type.
        /// If <paramref name="componentType"/> is generic this logic repeats recursively for each generic parameter.
        /// </remarks>
        public static string GetCategoryNameGeneric(Type componentType)
        {
            var result = new StringBuilder();

            var hasNested = false;

            void BuildPrettyName(Type t)
            {
                var name = GetLoggerName(t);
                var genericArgs = t.GetGenericArguments();

                if (genericArgs.Length == 0)
                {
                    result.Append(name);

                    hasNested |= t.IsNested;
                }
                else
                {
                    var baseNameIndex = name.IndexOf('`');

                    result.Append(name, 0, baseNameIndex >= 0 ? baseNameIndex : name.Length);

                    result.Append('<');

                    BuildPrettyName(genericArgs[0]);

                    for (var i = 1; i < genericArgs.Length; ++i)
                    {
                        result.Append(',');

                        BuildPrettyName(genericArgs[i]);
                    }

                    result.Append('>');
                }
            }

            BuildPrettyName(componentType);

            if (hasNested)
            {
                result.Replace('+', '.');
            }

            return result.ToString();
        }


        private static string GetLoggerName(Type type)
        {
            var attribute = type.GetTypeInfo().GetCustomAttribute<LoggerNameAttribute>();

            return string.IsNullOrEmpty(attribute?.Name) ? type.FullName : attribute.Name;
        }
    }
}