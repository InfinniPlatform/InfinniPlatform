using System;
using System.Collections;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Dynamic;

using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Реализует логику сериализации и десериализации <see cref="DynamicWrapper"/> для MongoDB.
    /// </summary>
    internal sealed class MongoDynamicWrapperBsonSerializer : SerializerBase<DynamicWrapper>
    {
        public static readonly MongoDynamicWrapperBsonSerializer Default = new MongoDynamicWrapperBsonSerializer();


        private static readonly IBsonSerializer<object> ObjectSerializer;
        private static readonly IBsonSerializer<List<object>> ListSerializer;


        static MongoDynamicWrapperBsonSerializer()
        {
            ObjectSerializer = BsonSerializer.LookupSerializer<object>();
            ListSerializer = BsonSerializer.LookupSerializer<List<object>>();
        }


        public override DynamicWrapper Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var reader = context.Reader;

            var currentBsonType = reader.GetCurrentBsonType();

            if (currentBsonType != BsonType.Document)
            {
                throw new FormatException($"Cannot deserialize a '{BsonUtils.GetFriendlyTypeName(typeof(DynamicWrapper))}' from BsonType '{currentBsonType}'.");
            }

            var dynamicDeserializationContext = context.With(ConfigureDeserializationContext);

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
                    memberValue = ObjectSerializer.Deserialize(dynamicDeserializationContext);
                }

                document[memberName] = memberValue;
            }

            reader.ReadEndDocument();

            return document;
        }


        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DynamicWrapper value)
        {
            var writer = context.Writer;

            var dynamicSerializationContext = context.With(ConfigureSerializationContext);

            writer.WriteStartDocument();

            foreach (KeyValuePair<string, object> property in value)
            {
                if (property.Value != null)
                {
                    writer.WriteName(property.Key);

                    ObjectSerializer.Serialize(dynamicSerializationContext, property.Value);
                }
            }

            writer.WriteEndDocument();
        }


        private void ConfigureDeserializationContext(BsonDeserializationContext.Builder builder)
        {
            builder.DynamicDocumentSerializer = this;
            builder.DynamicArraySerializer = ListSerializer;
        }

        private static void ConfigureSerializationContext(BsonSerializationContext.Builder builder)
        {
            builder.IsDynamicType = t => (t == typeof(DynamicWrapper)) || (t != typeof(string) && typeof(IEnumerable).IsAssignableFrom(t));
        }
    }
}