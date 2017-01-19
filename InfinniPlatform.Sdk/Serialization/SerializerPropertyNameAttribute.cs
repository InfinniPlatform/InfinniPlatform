using System;

namespace InfinniPlatform.Sdk.Serialization
{
    /// <summary>
    /// Определяет имя свойства для сериализатора <see cref="IObjectSerializer" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SerializerPropertyNameAttribute : Attribute
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Имя свойства.</param>
        public SerializerPropertyNameAttribute(string name)
        {
            Name = name;
        }


        /// <summary>
        /// Имя свойства.
        /// </summary>
        public string Name { get; }
    }
}