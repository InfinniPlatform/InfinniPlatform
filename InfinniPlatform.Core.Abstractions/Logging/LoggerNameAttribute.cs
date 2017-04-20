using System;

namespace InfinniPlatform.Logging
{
    /// <summary>
    /// Атрибут для определения имени источника событий.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class LoggerNameAttribute : Attribute
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя источника событий.</param>
        public LoggerNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Имя источника событий.
        /// </summary>
        public string Name { get; }
    }
}