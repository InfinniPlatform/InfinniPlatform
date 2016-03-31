using System;

using InfinniPlatform.Sdk.Types;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Реализует логику сериализации и десериализации <see cref="Date"/> для MongoDB.
    /// </summary>
    internal sealed class MongoDateBsonSerializer : MongoBsonSerializerBase<Date>
    {
        public static readonly MongoDateBsonSerializer Default = new MongoDateBsonSerializer();


        protected override void SerializeValue(BsonSerializationContext context, object value)
        {
            var date = (Date)value;
            var writer = context.Writer;
            writer.WriteInt64(date.UnixTime);
        }

        protected override object DeserializeValue(BsonDeserializationContext context)
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
                    throw new FormatException($"Cannot deserialize a '{BsonUtils.GetFriendlyTypeName(ValueType)}' from BsonType '{currentBsonType}'.");
            }

            return new Date(unixTime);
        }
    }
}