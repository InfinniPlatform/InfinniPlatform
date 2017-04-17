using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Abstractions.Dynamic;

using Newtonsoft.Json;

namespace InfinniPlatform.Core.Serialization
{
    /// <summary>
    /// Осуществляет преобразование объекта в JSON-представление и обратно для случаев, когда тип объекта заранее не известен <see cref="object"/>.
    /// </summary>
    internal class ObjectMemberJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Does not change the serialization logic
            serializer.Serialize(writer, value);
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                // JObject is interpreted as DynamicWrapper
                return serializer.Deserialize(reader, typeof(DynamicWrapper));
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                // JArray is interpreted as List<object>
                return serializer.Deserialize(reader, typeof(List<object>));
            }

            // Default value
            return reader.Value;
        }
    }
}