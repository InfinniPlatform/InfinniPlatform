using System;
using System.Reflection;

using InfinniPlatform.Dynamic;

using Microsoft.Extensions.Logging.Abstractions.Internal;

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
        /// If it is marked returns <see cref="LoggerNameAttribute.Name" /> otherwise a result of
        /// the <see cref="TypeNameHelper.GetTypeDisplayName" /> method.
        /// </remarks>
        public static string GetCategoryName(Type componentType)
        {
            var categoryName = componentType.GetTypeInfo().GetAttributeValue<LoggerNameAttribute, string>(i => i.Name, TypeNameHelper.GetTypeDisplayName(componentType));

            return categoryName;
        }
    }
}