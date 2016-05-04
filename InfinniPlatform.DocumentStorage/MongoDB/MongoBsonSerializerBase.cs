using System;
using System.Collections;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Dynamic;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    /// <summary>
    /// Реализует базовую логику сериализации и десериализации для MongoDB.
    /// </summary>
    internal abstract class MongoBsonSerializerBase : IBsonSerializer, IBsonPolymorphicSerializer
    {
        private static readonly IBsonSerializer<object> ObjectSerializer;
        private static readonly IBsonSerializer<List<object>> ListSerializer;


        static MongoBsonSerializerBase()
        {
            ObjectSerializer = BsonSerializer.LookupSerializer<object>();
            ListSerializer = BsonSerializer.LookupSerializer<List<object>>();
        }


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="valueType">Тип значения.</param>
        protected MongoBsonSerializerBase(Type valueType)
        {
            ValueType = valueType;
        }


        /// <summary>
        /// Тип значения.
        /// </summary>
        public Type ValueType { get; }

        /// <summary>
        /// Определяет, что тип <see cref="ValueType"/> в динамическом контексте следует интерпретировать,
        /// как известный, а экземпляры этого типа следует сохранять с использованием текущего сериализатора.
        /// В противном случае экземпляр типа <see cref="ValueType"/> будет сохранен в обертке { _t, _v } с
        /// использованием стандартного сериализатора для объектов.
        /// </summary>
        public bool IsDiscriminatorCompatibleWithObjectSerializer => true;


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
            var nominalType = value?.GetType() ?? ObjectSerializer.ValueType;

            ObjectSerializer.Serialize(context,
                                       new BsonSerializationArgs
                                       {
                                           NominalType = nominalType
                                       },
                                       value);
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
            return BsonSerializer.LookupSerializer(valueType).Deserialize(context);
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