using System;

using InfinniPlatform.Types;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// Converts <see cref="Date"/> into JSON and vice versa.
    /// </summary>
    public class DateJsonConverter : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Date) || objectType == typeof(Date?));
        }


        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Date convertValue)
            {
                writer.WriteValue(convertValue.UnixTime);
            }
            else
            {
                writer.WriteNull();
            }
        }


        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (JToken.Load(reader) is JValue jConvertValue 
                && (jConvertValue.Type == JTokenType.Integer || jConvertValue.Type == JTokenType.Float))
            {
                return new Date((long)jConvertValue.Value);
            }

            return null;
        }
    }
}