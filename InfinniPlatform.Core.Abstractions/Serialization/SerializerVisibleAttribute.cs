using System;

namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// Определяет, что свойство класса видимо для сериализатора <see cref="IObjectSerializer" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SerializerVisibleAttribute : Attribute
    {
    }
}