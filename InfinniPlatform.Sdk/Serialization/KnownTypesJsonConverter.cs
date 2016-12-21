using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Properties;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Sdk.Serialization
{
    /// <summary>
    /// Осуществляет преобразование объекта в JSON-представление и обратно на основе списка известных типов.
    /// </summary>
    internal sealed class KnownTypesJsonConverter : JsonConverter
    {
        public KnownTypesJsonConverter(KnownTypesContainer knownTypes)
        {
            _enumerableType = typeof(IEnumerable);
            _knownTypes = knownTypes ?? new KnownTypesContainer();
        }

        private readonly Type _enumerableType;
        private readonly KnownTypesContainer _knownTypes;

        public override bool CanRead => true;

        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType)
        {
            return _knownTypes.HasType(objectType) ||
                   ((objectType.GetTypeInfo().IsInterface || objectType.GetTypeInfo().IsAbstract) && !_enumerableType.GetTypeInfo().IsAssignableFrom(objectType));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jObj = JToken.FromObject(value);

            if (jObj.Type == JTokenType.Object)
            {
                var objType = value.GetType();
                var objTypeName = _knownTypes.GetName(objType);

                var jObjProperty = new JProperty(objTypeName, jObj);
                var jObjWrapper = new JObject { jObjProperty };

                WrapObjectProperties((JObject)jObj, value, serializer);

                jObj = jObjWrapper;
            }

            jObj.WriteTo(writer);
        }

        private static void WrapObjectProperties(JObject jObj, object value, JsonSerializer serializer)
        {
            var objType = value.GetType();

            foreach (var jProperty in jObj.Properties())
            {
                var objProperty = objType.GetProperty(jProperty.Name);

                var objPropertyValue = (objProperty as PropertyInfo)?.GetValue(value);

                if (objPropertyValue != null)
                {
                    jProperty.Value = JToken.FromObject(objPropertyValue, serializer);
                }
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObjWrapper = JObject.Load(reader);
            var jObjProperty = jObjWrapper.Properties().FirstOrDefault();

            if (jObjProperty != null)
            {
                var objTypeName = jObjProperty.Name;
                var objType = _knownTypes.GetType(objTypeName);

                if (objType != null)
                {
                    var obj = Activator.CreateInstance(objType, true);
                    serializer.Populate(jObjProperty.Value.CreateReader(), obj);

                    return obj;
                }
            }

            throw new InvalidOperationException(string.Format(Resources.CannotDeserializeTypeError, objectType.FullName));
        }
    }
}