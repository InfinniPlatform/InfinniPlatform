using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Sdk.Dynamic
{
    /// <summary>
    /// Осуществляет преобразование <see cref="DynamicWrapper"/> в JSON-представление и обратно на основе списка известных типов.
    /// </summary>
    internal sealed class DynamicWrapperJsonConverter : JsonConverter
    {
        private static readonly Type DynamicWrapperType = typeof (DynamicWrapper);


        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }


        public override bool CanConvert(Type objectType)
        {
            return (objectType == DynamicWrapperType);
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


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jObj = JObject.Load(reader);

            return DynamicWrapperExtensions.ConvertFromJsonObject(jObj);
        }
    }
}