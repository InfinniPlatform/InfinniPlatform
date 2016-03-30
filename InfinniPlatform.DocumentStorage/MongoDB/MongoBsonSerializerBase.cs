using System;
using System.Collections;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Dynamic;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    internal abstract class MongoBsonSerializerBase : IBsonSerializer
    {
        private static readonly IBsonSerializer<object> ObjectSerializer;
        private static readonly IBsonSerializer<List<object>> ListSerializer;


        static MongoBsonSerializerBase()
        {
            ObjectSerializer = BsonSerializer.LookupSerializer<object>();
            ListSerializer = BsonSerializer.LookupSerializer<List<object>>();
        }


        protected MongoBsonSerializerBase(Type valueType)
        {
            ValueType = valueType;
        }


        public Type ValueType { get; }


        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            var serializationContext = context.With(ConfigureSerializationContext);

            SerializeValue(serializationContext, value);
        }

        private static void ConfigureSerializationContext(BsonSerializationContext.Builder builder)
        {
            builder.IsDynamicType = t => (t == typeof(DynamicWrapper)) || (t != typeof(string) && typeof(IEnumerable).IsAssignableFrom(t));
        }

        protected void WriteValue(BsonSerializationContext context, object value)
        {
            ObjectSerializer.Serialize(context, value);
        }

        protected abstract void SerializeValue(BsonSerializationContext context, object value);


        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var deserializationContext = context.With(ConfigureDeserializationContext);

            return DeserializeValue(deserializationContext);
        }

        private void ConfigureDeserializationContext(BsonDeserializationContext.Builder builder)
        {
            builder.DynamicDocumentSerializer = this;
            builder.DynamicArraySerializer = ListSerializer;
        }

        protected object ReadValue(BsonDeserializationContext context)
        {
            return ObjectSerializer.Deserialize(context);
        }

        protected object ReadValue(BsonDeserializationContext context, Type valueType)
        {
            return ObjectSerializer.Deserialize(context, new BsonDeserializationArgs { NominalType = valueType });
        }

        protected abstract object DeserializeValue(BsonDeserializationContext context);
    }


    internal abstract class MongoBsonSerializerBase<TValue> : MongoBsonSerializerBase, IBsonSerializer<TValue>
    {
        protected MongoBsonSerializerBase() : base(typeof(TValue))
        {
        }


        void IBsonSerializer<TValue>.Serialize(BsonSerializationContext context, BsonSerializationArgs args, TValue value)
        {
            Serialize(context, args, value);
        }

        TValue IBsonSerializer<TValue>.Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            return (TValue)Deserialize(context, args);
        }
    }
}