﻿using System;

namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    /// Атрибут для определения имени очереди для сообщения.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class QueueNameAttribute : Attribute
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя очереди сообщения.</param>
        public QueueNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Имя очереди сообщения.
        /// </summary>
        public string Name { get; }
    }
}