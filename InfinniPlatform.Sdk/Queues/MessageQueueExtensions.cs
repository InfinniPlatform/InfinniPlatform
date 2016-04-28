using System;

using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.Queues
{
    /// <summary>
    ///     Методы расширения для работы с абстракциями очереди сообщений.
    /// </summary>
    public static class MessageQueueExtensions
    {
        private static readonly JsonObjectSerializer Serializer = JsonObjectSerializer.Default;

        /// <summary>
        ///     Опубликовать сообщение.
        /// </summary>
        /// <typeparam name="T">Тип сообщения.</typeparam>
        /// <param name="target">Сервис для публикации сообщений.</param>
        /// <param name="exchange">Наименование точки обмена сообщениями.</param>
        /// <param name="routingKey">Ключ маршрутизации сообщения.</param>
        /// <param name="properties">Свойства сообщения.</param>
        /// <param name="body">Тело сообщения.</param>
        public static void Publish<T>(this IMessageQueuePublisher target, string exchange, string routingKey,
            MessageProperties properties, T body)
        {
            if (properties == null)
            {
                properties = new MessageProperties();
            }

            properties.TypeName = typeof (T).FullName;
            properties.ContentType = "application/json";
            properties.ContentEncoding = "utf-8";

            target.Publish(exchange, routingKey, properties, Serializer.Serialize(body));
        }

        /// <summary>
        ///     Задать значение свойства.
        /// </summary>
        /// <typeparam name="T">Тип значения свойства.</typeparam>
        /// <param name="target">Заголовок сообщения.</param>
        /// <param name="key">Ключ свойства.</param>
        /// <param name="value">Значение свойства.</param>
        public static void Set<T>(this MessageHeaders target, string key, T value)
        {
            target.Set(key, Serializer.Serialize(value));
        }

        /// <summary>
        ///     Получить значение свойства.
        /// </summary>
        /// <param name="target">Заголовок сообщения.</param>
        /// <param name="key">Ключ свойства.</param>
        /// <param name="type">Тип значения свойства.</param>
        /// <returns>Значение свойства.</returns>
        public static object Get(this MessageHeaders target, string key, Type type)
        {
            return Serializer.Deserialize(target.Get(key), type);
        }

        /// <summary>
        ///     Получить значение свойства.
        /// </summary>
        /// <typeparam name="T">Тип значения свойства.</typeparam>
        /// <param name="target">Заголовок сообщения.</param>
        /// <param name="key">Ключ свойства.</param>
        /// <returns>Значение свойства.</returns>
        public static T Get<T>(this MessageHeaders target, string key)
        {
            return (T) Serializer.Deserialize(target.Get(key), typeof (T));
        }

        /// <summary>
        ///     Задать объект тела сообщения.
        /// </summary>
        /// <typeparam name="T">Тип тела сообщения.</typeparam>
        /// <param name="target">Сообщение очереди.</param>
        /// <param name="value">Объект тела сообщения.</param>
        public static void SetBodyObject<T>(this Message target, T value)
        {
            target.Body = Serializer.Serialize(value);
        }

        /// <summary>
        ///     Получить объект тела сообщения.
        /// </summary>
        /// <param name="target">Сообщение очереди.</param>
        /// <param name="type">Тип тела сообщения.</param>
        /// <returns>Объект тела сообщения.</returns>
        public static object GetBodyObject(this Message target, Type type)
        {
            return Serializer.Deserialize(target.Body, type);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T">Тип тела сообщения.</typeparam>
        /// <param name="target">Сообщение очереди.</param>
        /// <returns>Объект тела сообщения.</returns>
        public static T GetBodyObject<T>(this Message target)
        {
            return (T) Serializer.Deserialize(target.Body, typeof (T));
        }
    }
}