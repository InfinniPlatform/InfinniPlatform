using System;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage
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
                case BsonType.Document:
                    // Workaroud for deserializing values like:
                    // { "_t" : "System.Decimal", "_v" : "99.9" }
                    decimal128 = ReadDecimalFromDocument(reader, currentBsonType);
                    break;
                default:
                    throw new FormatException($"Cannot deserialize a '{BsonUtils.GetFriendlyTypeName(ValueType)}' from BsonType '{currentBsonType}'.");
            }

            return (decimal)decimal128;
        }

        private Decimal128 ReadDecimalFromDocument(IBsonReader reader, BsonType currentBsonType)
        {
            reader.ReadStartDocument();

            var memberName = reader.ReadName();
            var stringValue = reader.ReadString();

            if (memberName == "_t" && stringValue == typeof(decimal).FullName)
            {
                memberName = reader.ReadName();

                if (memberName == "_v")
                {
                    var value = reader.ReadString();
                    reader.ReadEndDocument();

                    return Decimal128.Parse(value);
                }
            }

            throw new FormatException($"Cannot deserialize a '{BsonUtils.GetFriendlyTypeName(ValueType)}' from BsonType '{currentBsonType}'.");
        }
    }
}