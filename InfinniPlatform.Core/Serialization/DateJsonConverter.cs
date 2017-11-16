using System;

using InfinniPlatform.Types;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// Осуществляет преобразование <see cref="Date"/> в JSON-представление и обратно.
    /// </summary>
    public class DateJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Date) || objectType == typeof(Date?));
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Date)
            {
                var convertValue = (Date)value;

                writer.WriteValue(convertValue.UnixTime);
            }
            else
            {
                writer.WriteNull();
            }
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jConvertValue = JToken.Load(reader) as JValue;

            if (jConvertValue != null && (jConvertValue.Type == JTokenType.Integer || jConvertValue.Type == JTokenType.Float))
            {
                return new Date((long)jConvertValue.Value);
            }

            return null;
        }
    }
}