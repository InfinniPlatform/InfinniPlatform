using System;
using System.Collections.Generic;

using InfinniPlatform.Dynamic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// Осуществляет преобразование <see cref="DynamicWrapper"/> в JSON-представление и обратно.
    /// </summary>
    internal class DynamicWrapperJsonConverter : JsonConverter
    {
        private static readonly Type ConvertType = typeof(DynamicWrapper);


        public override bool CanConvert(Type objectType)
        {
            return (objectType == ConvertType);
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dynamicWrapper = value as DynamicWrapper;

            if (dynamicWrapper != null)
            {
                writer.WriteStartObject();

                foreach (KeyValuePair<string, object> property in dynamicWrapper)
                {
                    writer.WritePropertyName(property.Key);
                    serializer.Serialize(writer, property.Value);
                }

                writer.WriteEndObject();
            }
            else
            {
                writer.WriteNull();
            }
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObj = JObject.Load(reader);

            return ConvertFromJsonObject(jObj);
        }


        private static object ConvertFromJsonObject(JObject value)
        {
            DynamicWrapper result = null;

            if (value != null)
            {
                result = new DynamicWrapper();

                foreach (var property in value)
                {
                    if (property.Value != null)
                    {
                        result[property.Key] = ConvertFromJsonToken(property.Value);
                    }
                }
            }

            return result;
        }

        private static object ConvertFromJsonToken(JToken value)
        {
            object result = null;

            if (value != null)
            {
                if (value is JValue)
                {
                    result = ((JValue)value).Value;
                }
                else if (value is JObject)
                {
                    result = ConvertFromJsonObject((JObject)value);
                }
                else if (value is JArray)
                {
                    var array = new List<object>();

                    foreach (var item in (JArray)value)
                    {
                        array.Add(ConvertFromJsonToken(item));
                    }

                    result = array;
                }
            }

            return result;
        }
    }
}