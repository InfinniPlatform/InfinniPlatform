using System;

using InfinniPlatform.Sdk.Types;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Реализует логику сериализации и десериализации <see cref="Date"/> для MongoDB.
    /// </summary>
    internal sealed class MongoDateBsonSerializer : SerializerBase<Date>
    {
        public static readonly MongoDateBsonSerializer Default = new MongoDateBsonSerializer();

        public override Date Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var reader = context.Reader;

            var currentBsonType = reader.GetCurrentBsonType();

            long unixTime;

            switch (currentBsonType)
            {
                case BsonType.Int64:
                    unixTime = reader.ReadInt64();
                    break;
                case BsonType.Int32:
                    unixTime = reader.ReadInt32();
                    break;
                case BsonType.Double:
                    unixTime = (long)reader.ReadDouble();
                    break;
                default:
                    throw new FormatException($"Cannot deserialize a '{BsonUtils.GetFriendlyTypeName(typeof(Date))}' from BsonType '{currentBsonType}'.");
            }

            return new Date(unixTime);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Date value)
        {
            var writer = context.Writer;

            writer.WriteInt64(value.UnixTime);
        }
    }
}