using System;

using InfinniPlatform.Sdk.Types;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Реализует логику сериализации и десериализации <see cref="Time"/> для MongoDB.
    /// </summary>
    internal sealed class MongoTimeBsonSerializer : MongoBsonSerializerBase<Time>, IBsonPolymorphicSerializer
    {
        public static readonly MongoTimeBsonSerializer Default = new MongoTimeBsonSerializer();


        /// <summary>
        /// Определяет, что тип <see cref="ValueType"/> в динамическом контексте следует интерпретировать,
        /// как известный, а экземпляры этого типа следует сохранять с использованием текущего сериализатора.
        /// В противном случае экземпляр типа <see cref="ValueType"/> будет сохранен в обертке { _t, _v } с
        /// использованием стандартного сериализатора для объектов.
        /// </summary>
        public bool IsDiscriminatorCompatibleWithObjectSerializer => true;


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