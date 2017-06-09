using System;

using Microsoft.Extensions.Logging;

namespace InfinniPlatform.Logging
{
    /// <summary>
    /// The attribute defines the category name for the <see cref="ILogger{TCategoryName}" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
    public class LoggerNameAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="LoggerNameAttribute" />.
        /// </summary>
        /// <param name="name">The logger category name.</param>
        public LoggerNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The logger category name.
        /// </summary>
        public string Name { get; }
    }
}