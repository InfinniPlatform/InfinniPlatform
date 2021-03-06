﻿using System;
using System.Collections.Generic;
using System.Reflection;

using InfinniPlatform.Dynamic;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Serialization
{
    /// <summary>
    /// Converts <see cref="DynamicDocument"/> into JSON and vice versa.
    /// </summary>
    public class DynamicDocumentJsonConverter : JsonConverter
    {
        private static readonly TypeInfo ConvertType = typeof(DynamicDocument).GetTypeInfo();


        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return ConvertType.IsAssignableFrom(objectType);
        }


        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dynamicDocument = value as DynamicDocument;

            if (dynamicDocument != null)
            {
                writer.WriteStartObject();

                foreach (KeyValuePair<string, object> property in dynamicDocument)
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


        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObj = JObject.Load(reader);

            return ConvertFromJsonObject(jObj);
        }


        private static object ConvertFromJsonObject(JObject value)
        {
            DynamicDocument result = null;

            if (value != null)
            {
                result = new DynamicDocument();

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