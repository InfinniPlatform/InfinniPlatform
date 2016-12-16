using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Dynamic;

using Newtonsoft.Json;

namespace InfinniPlatform.Sdk.Serialization
{
    /// <summary>
    /// Осуществляет преобразование объекта в JSON-представление и обратно для случаев, когда тип объекта заранее не известен <see cref="object"/>.
    /// </summary>
    internal class JsonObjectMemberConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Логика сериализации по умолчанию
            serializer.Serialize(writer, value);
        }


        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                // JObject интерпретируются, как DynamicWrapper
                return serializer.Deserialize(reader, typeof(DynamicWrapper));
            }

            if (reader.TokenType == JsonToken.StartArray)
            {
                // JArray интерпретируются, как List<object>
                return serializer.Deserialize(reader, typeof(List<object>));
            }

            // Значение по умолчанию
            return reader.Value;
        }
    }
}