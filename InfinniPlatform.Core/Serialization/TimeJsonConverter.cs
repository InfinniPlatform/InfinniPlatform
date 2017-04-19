using System;

using InfinniPlatform.Core.Types;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Core.Serialization
{
    /// <summary>
    /// Осуществляет преобразование <see cref="Time"/> в JSON-представление и обратно.
    /// </summary>
    internal class TimeJsonConverter : JsonConverter
    {
        private static readonly Type ConvertType = typeof(Time);


        public override bool CanConvert(Type objectType)
        {
            return (objectType == ConvertType);
        }


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