using System;

using InfinniPlatform.Types;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Предоставляет интерфейс сериализации и десериализации <see cref="Time"/> для MongoDB.
    /// </summary>
    internal class MongoTimeBsonSerializationProvider : IBsonSerializationProvider
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