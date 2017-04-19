using System;

using Newtonsoft.Json;

namespace InfinniPlatform.Core.Serialization
{
    /// <summary>
    /// Осуществляет преобразование объекта в JSON-представление и обратно на основе <see cref="IMemberValueConverter"/>.
    /// </summary>
    internal class MemberValueJsonConverter : JsonConverter
    {
        public MemberValueJsonConverter(bool canRead, bool canWrite, IMemberValueConverter valueConverter)
        {
            CanRead = canRead;
            CanWrite = canWrite;

            _valueConverter = valueConverter;
        }


        private readonly IMemberValueConverter _valueConverter;


        public override bool CanRead { get; }

        public override bool CanWrite { get; }


        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var convertedValue = _valueConverter.Convert(value);
            serializer.Serialize(writer, convertedValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var convertedValue = _valueConverter.ConvertBack(t => serializer.Deserialize(reader, t));
            return convertedValue;
        }
    }
}