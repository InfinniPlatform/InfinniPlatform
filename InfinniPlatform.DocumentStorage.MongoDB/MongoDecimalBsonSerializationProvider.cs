using System;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage
{
    internal class MongoDecimalBsonSerializationProvider : IBsonSerializationProvider
    {
        public static readonly MongoDecimalBsonSerializationProvider Default = new MongoDecimalBsonSerializationProvider();


        public IBsonSerializer GetSerializer(Type type)
        {
            if (type == typeof(decimal))
            {
                return MongoDecimalBsonSerializer.Default;
            }

            return null;
        }
    }
}