using System;

namespace InfinniPlatform.Sdk.Queues
{
    /// <summary>
    /// Атрибут для определения имени очереди для сообщения.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class QueueNameAttribute : Attribute
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="value">Имя очереди сообщения.</param>
        public QueueNameAttribute(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Имя очереди сообщения.
        /// </summary>
        public string Value { get; }
    }
}