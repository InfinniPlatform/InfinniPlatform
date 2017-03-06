using System;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal class MongoDecimalBsonSerializer : MongoBsonSerializerBase<decimal>
    {
        public static readonly MongoDecimalBsonSerializer Default = new MongoDecimalBsonSerializer();


        protected override void SerializeValue(BsonSerializationContext context, object value)
        {
            var number = (decimal)value;
            var writer = context.Writer;
            writer.WriteDecimal128(number);
        }

        protected override object DeserializeValue(BsonDeserializationContext context)
        {
            var reader = context.Reader;

            var currentBsonType = reader.GetCurrentBsonType();

            Decimal128 decimal128;

            switch (currentBsonType)
            {
                case BsonType.Decimal128:
                    decimal128 = reader.ReadDecimal128();
                    break;
                default:
                    throw new FormatException($"Cannot deserialize a '{BsonUtils.GetFriendlyTypeName(ValueType)}' from BsonType '{currentBsonType}'.");
            }

            return (decimal)decimal128;
        }
    }
}