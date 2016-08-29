using System;

namespace InfinniPlatform.Scheduler.Contract
{
    /// <summary>
    /// Предоставляет метод для сериализации и десериализации типов обработчиков заданий <see cref="IJobHandler"/>.
    /// </summary>
    public interface IJobHandlerTypeSerializer
    {
        /// <summary>
        /// Сериализует тип обработчика заданий.
        /// </summary>
        string Serialize(Type type);

        /// <summary>
        /// Десериализует тип обработчика заданий.
        /// </summary>
        Type Deserialize(string value);
    }
}