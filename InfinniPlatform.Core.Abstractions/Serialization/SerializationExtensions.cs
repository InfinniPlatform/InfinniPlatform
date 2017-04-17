using System.IO;

namespace InfinniPlatform.Core.Abstractions.Serialization
{
    public static class SerializationExtensions
    {
        public static T Deserialize<T>(this IObjectSerializer target, byte[] data)
        {
            return (T)target.Deserialize(data, typeof(T));
        }

        public static T Deserialize<T>(this IObjectSerializer target, Stream data)
        {
            return (T)target.Deserialize(data, typeof(T));
        }

        public static T Deserialize<T>(this IJsonObjectSerializer target, string data)
        {
            return (T)target.Deserialize(data, typeof(T));
        }

        public static T ConvertFromDynamic<T>(this IJsonObjectSerializer target, object value)
        {
            return (T)target.ConvertFromDynamic(value, typeof(T));
        }
    }
}