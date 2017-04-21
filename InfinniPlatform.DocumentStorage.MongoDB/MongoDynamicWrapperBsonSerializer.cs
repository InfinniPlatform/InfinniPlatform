using System;
using System.Collections.Generic;

using InfinniPlatform.Dynamic;

using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Реализует логику сериализации и десериализации <see cref="DynamicWrapper"/> для MongoDB.
    /// </summary>
    internal class MongoDynamicWrapperBsonSerializer : MongoBsonSerializerBase<DynamicWrapper>
    {
        public static readonly MongoDynamicWrapperBsonSerializer Default = new MongoDynamicWrapperBsonSerializer();


        protected override void SerializeValue(BsonSerializationContext context, object value)
        {
            var document = (DynamicWrapper)value;

            var writer = context.Writer;

            writer.WriteStartDocument();

            foreach (KeyValuePair<string, object> property in document)
            {
                if (property.Value != null)
                {
                    writer.WriteName(property.Key);

                    WriteValue(context, property.Value);
                }
            }

            writer.WriteEndDocument();
        }

        protected override object DeserializeValue(BsonDeserializationContext context)
        {
            var reader = context.Reader;

            var currentBsonType = reader.GetCurrentBsonType();

            if (currentBsonType != BsonType.Document)
            {
                throw new FormatException($"Cannot deserialize a '{BsonUtils.GetFriendlyTypeName(ValueType)}' from BsonType '{currentBsonType}'.");
            }

            reader.ReadStartDocument();

            var document = new DynamicWrapper();

            while (reader.ReadBsonType() != BsonType.EndOfDocument)
            {
                var memberName = reader.ReadName();
                var memberBsonType = reader.GetCurrentBsonType();

                object memberValue;

                if (memberBsonType == BsonType.Binary)
                {
                    var bsonBinaryData = reader.ReadBinaryData();

                    switch (bsonBinaryData.SubType)
                    {
                        case BsonBinarySubType.UuidLegacy:
                        case BsonBinarySubType.UuidStandard:
                            memberValue = bsonBinaryData.ToGuid();
                            break;
                        default:
                            memberValue = bsonBinaryData.Bytes;
                            break;
                    }
                }
                else
                {
                    memberValue = ReadValue(context);
                }

                document[memberName] = memberValue;
            }

            reader.ReadEndDocument();

            return document;
        }
    }
}