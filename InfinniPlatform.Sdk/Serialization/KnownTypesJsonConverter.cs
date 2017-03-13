using System;

using InfinniPlatform.Sdk.Properties;

using Newtonsoft.Json;

namespace InfinniPlatform.Sdk.Serialization
{
    /// <summary>
    /// Осуществляет преобразование объекта в JSON-представление и обратно на основе списка известных типов.
    /// </summary>
    internal class KnownTypesJsonConverter : JsonConverter
    {
        public KnownTypesJsonConverter(KnownTypesContainer knownTypes)
        {
            _knownTypes = knownTypes;
        }


        private readonly KnownTypesContainer _knownTypes;


        public override bool CanRead => true;

        public override bool CanWrite => true;


        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
            {
                var valueType = value.GetType();

                var valueTypeName = _knownTypes.GetName(valueType);

                if (!string.IsNullOrEmpty(valueTypeName))
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName(valueTypeName);
                    serializer.Serialize(writer, value);
                    writer.WriteEndObject();
                }
                else
                {
                    writer.WriteValue(value);
                }
            }
            else
            {
                writer.WriteNull();
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // JsonToken.PropertyName
            reader.Read();

            var valueTypeName = reader.Value as string;

            if (!string.IsNullOrEmpty(valueTypeName))
            {
                var valueType = _knownTypes.GetType(valueTypeName);

                if (valueType != null)
                {
                    // JsonToken.StartObject
                    reader.Read();

                    var value = serializer.Deserialize(reader, valueType);

                    // JsonToken.EndObject
                    reader.Read();

                    return value;
                }
            }

            throw new InvalidOperationException(string.Format(Resources.CannotDeserializeTypeError, objectType.FullName));
        }
    }
}