using System;

using InfinniPlatform.Core.Serialization;

using MongoDB.Bson.Serialization;

namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Реализует логику сериализации и десериализации для MongoDB на основе <see cref="IMemberValueConverter"/>.
    /// </summary>
    internal class MongoMemberValueConverter : MongoBsonSerializerBase
    {
        public MongoMemberValueConverter(Type memberType, IMemberValueConverter converter) : base(memberType)
        {
            _converter = converter;
        }


        private readonly IMemberValueConverter _converter;


        protected override void SerializeValue(BsonSerializationContext context, object value)
        {
            var convertedValue = _converter.Convert(value);
            WriteValue(context, convertedValue);
        }

        protected override object DeserializeValue(BsonDeserializationContext context)
        {
            var convertedValue = _converter.ConvertBack(valueType => ReadValue(context, valueType));
            return convertedValue;
        }
    }
}