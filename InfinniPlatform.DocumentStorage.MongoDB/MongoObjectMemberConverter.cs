using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Dynamic;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Реализует логику сериализации и десериализации для MongoDB в случаях, когда тип объекта заранее не известен <see cref="object"/>.
    /// </summary>
    internal class MongoObjectMemberConverter : MongoBsonSerializerBase, IBsonSerializer<object>
    {
        public MongoObjectMemberConverter(Type memberType) : base(memberType)
        {
        }


        protected override void SerializeValue(BsonSerializationContext context, object value)
        {
            WriteValue(context, value);
        }

        protected override object DeserializeValue(BsonDeserializationContext context)
        {
            if (context.Reader.CurrentBsonType == BsonType.Document)
            {
                // В случае BsonType.Document тип object интерпретируется, как DynamicWrapper
                return ReadValue(context, typeof(DynamicWrapper));
            }

            if (context.Reader.CurrentBsonType == BsonType.Array)
            {
                // В случае BsonType.Array тип object интерпретируется, как List<object>
                return ReadValue(context, typeof(List<object>));
            }

            return ReadValue(context, ValueType);
        }
    }
}