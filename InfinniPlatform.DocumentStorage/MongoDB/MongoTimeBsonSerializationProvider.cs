using System;

using InfinniPlatform.Sdk.Types;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Предоставляет интерфейс сериализации и десериализации <see cref="Time"/> для MongoDB.
    /// </summary>
    internal sealed class MongoTimeBsonSerializationProvider : IBsonSerializationProvider
    {
        public static readonly MongoTimeBsonSerializationProvider Default = new MongoTimeBsonSerializationProvider();


        public IBsonSerializer GetSerializer(Type type)
        {
            if (type == typeof(Time))
            {
                return MongoTimeBsonSerializer.Default;
            }

            return null;
        }
    }
}