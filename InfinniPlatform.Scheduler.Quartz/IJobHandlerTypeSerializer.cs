using System;

namespace InfinniPlatform.Scheduler
{
    /// <summary>
    /// Предоставляет метод для сериализации и десериализации типов обработчиков заданий <see cref="IJobHandler"/>.
    /// </summary>
    internal interface IJobHandlerTypeSerializer
    {
        /// <summary>
        /// Проверяет, возможно ли сериализовать тип обработчика заданий.
        /// </summary>
        bool CanSerialize(Type type);

        /// <summary>
        /// Сериализует тип обработчика заданий.
        /// </summary>
        string Serialize(Type type);

        /// <summary>
        /// Десериализует тип обработчика заданий.
        /// </summary>
        IJobHandler Deserialize(string value);
    }
}