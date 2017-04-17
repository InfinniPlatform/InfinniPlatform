using System;

using InfinniPlatform.Core.Abstractions.Types;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Реализует логику сериализации и десериализации <see cref="Time"/> для MongoDB.
    /// </summary>
    internal class MongoTimeBsonSerializer : MongoBsonSerializerBase<Time>
    {
        public static readonly MongoTimeBsonSerializer Default = new MongoTimeBsonSerializer();


        protected override void SerializeValue(BsonSerializationContext context, object value)
        {
            var time = (Time)value;
            var writer = context.Writer;
            writer.WriteDouble(time.TotalSeconds);
        }

        protected override object DeserializeValue(BsonDeserializationContext context)
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
                    throw new FormatException($"Cannot deserialize a '{BsonUtils.GetFriendlyTypeName(ValueType)}' from BsonType '{currentBsonType}'.");
            }

            return new Time(totalSeconds);
        }
    }
}