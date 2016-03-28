using System;

using InfinniPlatform.Sdk.Types;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Реализует логику сериализации и десериализации <see cref="Time"/> для MongoDB.
    /// </summary>
    internal sealed class MongoTimeBsonSerializer : SerializerBase<Time>
    {
        public static readonly MongoTimeBsonSerializer Default = new MongoTimeBsonSerializer();

        public override Time Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var reader = context.Reader;

            var currentBsonType = reader.GetCurrentBsonType();

            double totalSeconds;

            switch (currentBsonType)
            {
                case BsonType.Double:
                    totalSeconds = reader.ReadDouble();
                    break;
                case BsonType.Int64:
                    totalSeconds = reader.ReadInt64();
                    break;
                case BsonType.Int32:
                    totalSeconds = reader.ReadInt32();
                    break;
                default:
                    throw new FormatException($"Cannot deserialize a '{BsonUtils.GetFriendlyTypeName(typeof(Time))}' from BsonType '{currentBsonType}'.");
            }

            return new Time(totalSeconds);
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Time value)
        {
            var writer = context.Writer;

            writer.WriteDouble(value.TotalSeconds);
        }
    }
}