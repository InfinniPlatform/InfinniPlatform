using System;

using InfinniPlatform.Types;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// Converts <see cref="Time"/> into JSON and vice versa.
    /// </summary>
    public class TimeJsonConverter : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Time) || objectType == typeof(Time?));
        }


        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Time)
            {
                var convertValue = (Time)value;

                writer.WriteValue(convertValue.TotalSeconds);
            }
            else
            {
                writer.WriteNull();
            }
        }


        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jConvertValue = JToken.Load(reader) as JValue;

            if (jConvertValue != null && (jConvertValue.Type == JTokenType.Float || jConvertValue.Type == JTokenType.Integer))
            {
                return new Time((double)jConvertValue.Value);
            }

            return null;
        }
    }
}