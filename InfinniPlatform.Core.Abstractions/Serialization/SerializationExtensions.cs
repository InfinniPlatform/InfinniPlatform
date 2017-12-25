using System.IO;

namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// Extensions methods for <see cref="IObjectSerializer"/> and <see cref="IJsonObjectSerializer"/>.
    /// </summary>
    public static class SerializationExtensions
    {
        /// <summary>
        /// Deserializes instance of type <typeparamref name="T"/> from byte array.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="target">Serializer.</param>
        /// <param name="data">Byte array data.</param>
        public static T Deserialize<T>(this IObjectSerializer target, byte[] data)
        {
            return (T)target.Deserialize(data, typeof(T));
        }

        /// <summary>
        /// Deserializes instance of type <typeparamref name="T"/> from stream.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="target">Serializer.</param>
        /// <param name="data">Stream data.</param>
        public static T Deserialize<T>(this IObjectSerializer target, Stream data)
        {
            return (T)target.Deserialize(data, typeof(T));
        }

        /// <summary>
        /// Deserializes instance of type <typeparamref name="T"/> from string.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="target">Serializer.</param>
        /// <param name="data">String data.</param>
        public static T Deserialize<T>(this IJsonObjectSerializer target, string data)
        {
            return (T)target.Deserialize(data, typeof(T));
        }

        /// <summary>
        /// Converts dynamic object to instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="target">Serializer.</param>
        /// <param name="value">Dynamic object.</param>
        public static T ConvertFromDynamic<T>(this IJsonObjectSerializer target, object value)
        {
            return (T)target.ConvertFromDynamic(value, typeof(T));
        }
    }
}